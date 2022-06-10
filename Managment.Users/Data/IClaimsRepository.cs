using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public interface IClaimsRepository
{
    Task SaveChangesAsync();
    Task<IEnumerable<Claim>> GetUserClaimsAsync(IdentityUser user);
    Task AddClaimToUserAsync(IdentityUser user, Claim claim);
}