using Managment.Common.Extensions;
using Managment.Users.Services;
using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(UsersDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
    }
    public async Task CheckPasswordAsync(string email, string password)
    {
        if (await _userManager.CheckPasswordAsync(await GetUserByEmailAsync(email), password) is false)
        {
            throw new BadHttpRequestException($"User with email {email} has different password");
        }
    }

    public async Task CreateAsync(IdentityUser user,string password)
    {
        if (await _userManager.FindByEmailAsync(user.Email) is not null)
        {
            throw new BadHttpRequestException($"User with given email {user.Email} already exists");
        }
        var result = await _userManager.CreateAsync(user,password);
        result.CheckErrors();
    }

    public Task<IEnumerable<IdentityUser>> GetAllUsersAsync()
    {
        return Task.FromResult<IEnumerable<IdentityUser>>(_userManager.Users.ToList());
    }

    public async Task<IdentityUser> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new BadHttpRequestException($"User with given email {email} does not exists");
        }
        return user;
    }

    public async Task<IdentityUser> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            throw new BadHttpRequestException($"User with given id {id} does not exists");
        }
        return user;
    }

    public async Task RemoveUserAsync(string id)
    {
        var user = await GetUserByIdAsync(id);
        var result = await _userManager.DeleteAsync(user);
        result.CheckErrors();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}