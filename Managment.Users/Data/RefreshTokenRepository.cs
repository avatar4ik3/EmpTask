using System.IdentityModel.Tokens.Jwt;
using Managment.Users.Models;
using Managment.Users.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Managment.Users.Data;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly UsersDbContext _dbContext;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IUserRepository _userRepository;
    private readonly JwtGenerationService _jwtGenerationService;

    public RefreshTokenRepository(
        UsersDbContext dbContext,
         TokenValidationParameters tokenValidationParameters,
         IUserRepository userRepository,
         JwtGenerationService jwtGenerationService)
    {
        this._dbContext = dbContext;
        this._tokenValidationParameters = tokenValidationParameters;
        this._userRepository = userRepository;
        this._jwtGenerationService = jwtGenerationService;
    }
    public Task AddAsync(RefreshToken refreshToken)
    {
        return Task.FromResult(_dbContext.RefreshTokens.Add(refreshToken));
    }

    public async Task<TokenAndRefreshToken> RefreshAsync(string token, string refreshToken)
    {
        var oldRefreshToken = _dbContext.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken)!;
        await VerifyRefreshTokenAsync(token, oldRefreshToken);
        oldRefreshToken.IsUsed = true;
        _dbContext.RefreshTokens.Update(oldRefreshToken);
        await _dbContext.SaveChangesAsync();

        var user = await _userRepository.GetUserByIdAsync(oldRefreshToken.UserId);
        return await _jwtGenerationService.GenerateAsync(user);
    }

    public Task RemoveAsync(RefreshToken refreshToken)
    {
        return Task.FromResult(_dbContext.RefreshTokens.Remove(refreshToken));
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public Task VerifyRefreshTokenAsync(string token, RefreshToken storedToken)
    {
        return Task.Run(() =>
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {

                var tokenInVerification = jwtTokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (validatedToken is not JwtSecurityToken jwtSecurityToken) throw new Exception();
                var tokensComparingResult = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (tokensComparingResult is false) throw new Exception();
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

                var expiryDate = DateTime.UnixEpoch.AddSeconds(utcExpiryDate);

                var jti = tokenInVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken is null || storedToken.IsRevorked || storedToken.IsUsed || storedToken.JwtId != jti) throw new Exception();
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException("Refresh token is invalid");
            }
        });
    }

}