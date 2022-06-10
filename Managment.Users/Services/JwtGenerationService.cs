using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Managment.Common;
using Managment.Users.Data;
using Managment.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Managment.Users.Services;

public class JwtGenerationService
{
    private readonly JwtConfig _config;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public JwtGenerationService(
        JwtConfig config,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this._config = config;
        this._userManager = userManager;
        this._roleManager = roleManager;
    }

    public async Task<TokenAndRefreshToken> GenerateAsync(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.Secret);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]{
                new Claim("Id",user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            }),
            //TODO только для develop
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        //добавляем клеймы
        descriptor.Subject.AddClaims(await _userManager.GetClaimsAsync(user));
        //добавляем роли
        var userRoles = await _userManager.GetRolesAsync(user);
        descriptor.Subject.AddClaims(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

        //добавляем клеймы, прикинутые к ролям
        foreach (var userRole in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(userRole);
            if (role is not null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                descriptor.Subject.AddClaims(roleClaims);
            }
        }
        var token = jwtTokenHandler.CreateToken(descriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsUsed = false,
            IsRevorked = false,
            UserId = user.Id,
            AddedDate = DateTime.UtcNow,
            //TODO только для develop
            ExpiryDate = DateTime.UtcNow.AddMinutes(_config.DevelopRefreshTokenExpiryTimeMinutes),
            Token = Guid.NewGuid().ToString()
        };

        return new()
        {
            Token = jwtToken,
            RefreshToken = refreshToken
        };

    }
}