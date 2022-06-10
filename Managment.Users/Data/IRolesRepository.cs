using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public interface IRolesRepository
{
    Task SaveChangesAsync();
    Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
    Task<IdentityRole> GetRoleAsync(string roleId);
    Task<IdentityRole> CreateAsync(string role);
    Task AddUserToRole(IdentityUser user, IdentityRole role);
    Task<IEnumerable<string>> GetUserRolesAsync(IdentityUser user);
}