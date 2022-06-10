using Microsoft.AspNetCore.Identity;

namespace Managment.Users.Data;

public class InMemoryDbSeeder
{
    private readonly IRolesRepository _rolesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IClaimsRepository _claimsRepository;

    public InMemoryDbSeeder(IRolesRepository rolesRepository, IUserRepository userRepository, IClaimsRepository claimsRepository)
    {
        this._rolesRepository = rolesRepository;
        this._userRepository = userRepository;
        this._claimsRepository = claimsRepository;
    }

    public async Task SeedDb()
    {
        var user = new IdentityUser
        {
            UserName = "Admin",
            Email = "example@email.com"
        };
        await _userRepository.CreateAsync(user, "Pa55w0rd!");
        await _rolesRepository.CreateAsync("admin");
        await _claimsRepository.AddClaimToUserAsync(user,new System.Security.Claims.Claim("admin","admin"));
        await _userRepository.SaveChangesAsync();

    }
}