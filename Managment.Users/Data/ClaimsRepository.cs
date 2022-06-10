using System.Security.Claims;
using Managment.Common.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public class ClaimsRepository : IClaimsRepository
{
    private readonly UsersDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public ClaimsRepository(UsersDbContext dbContext,UserManager<IdentityUser> userManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
    }
    public async Task AddClaimToUserAsync(IdentityUser user, Claim claim)
    {
        var result = await _userManager.AddClaimAsync(user,claim);
        result.CheckErrors(); 
    }

    public async Task<IEnumerable<Claim>> GetUserClaimsAsync(IdentityUser user)
    {
        return (await _userManager.GetClaimsAsync(user)).ToList();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}