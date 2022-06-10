using Managment.Common.Extensions;
using Managment.Users.Services;
using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public class RolesRepository : IRolesRepository
{
    private readonly UsersDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RolesRepository(UsersDbContext dbContext,RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
    {
        this._dbContext = dbContext;
        this._roleManager = roleManager;
        this._userManager = userManager;
    }

    //TODO перенести бы в IUserRepository 
    public async Task AddUserToRole(IdentityUser user, IdentityRole role)
    {
        var result = await _userManager.AddToRoleAsync(user,role.Name);
        result.CheckErrors();
    }

    public async Task<IdentityRole> CreateAsync(string role)
    {
        var isRoleExists = await _roleManager.RoleExistsAsync(role);
        if(isRoleExists is true){
            throw new BadHttpRequestException($"Role with given name {role} already exists");
        }
        var identityRole = new IdentityRole(role); 
        var result = await _roleManager.CreateAsync(identityRole);
        result.CheckErrors();
        return identityRole;
    }

    public Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
    {
        return Task.FromResult<IEnumerable<IdentityRole>>(_roleManager.Roles.ToList());
    }

    public async Task<IdentityRole> GetRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if(role is null){
            throw new BadHttpRequestException($"Role with given id {roleId} does not exists");
        }
        return role;
    }

    //TODO тоже перенести бы в IUserRepository
    public async Task<IEnumerable<string>> GetUserRolesAsync(IdentityUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}