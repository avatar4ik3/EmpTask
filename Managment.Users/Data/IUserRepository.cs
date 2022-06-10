using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public interface IUserRepository
{
    Task CreateAsync(IdentityUser user,string password);

    Task<IEnumerable<IdentityUser>> GetAllUsersAsync();

    Task RemoveUserAsync(string id);
    Task SaveChangesAsync();
    Task CheckPasswordAsync(string email, string password);
    Task<IdentityUser> GetUserByEmailAsync(string email);
    Task<IdentityUser> GetUserByIdAsync(string id);
}