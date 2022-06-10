using Managment.Users.Models;

namespace Managment.Users.Data;

public interface IRefreshTokenRepository
{
    /*
    проверяет, валиден ли рефреш токен
    */
    Task VerifyRefreshTokenAsync(string token, RefreshToken refreshToken);

    Task AddAsync(RefreshToken refreshToken);

    Task RemoveAsync(RefreshToken refreshToken);

    Task SaveChangesAsync();
    Task<TokenAndRefreshToken> RefreshAsync(string token, string refreshToken);
}