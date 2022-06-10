namespace Managment.Common;

public class JwtConfig{
    public string Secret{get;set;}

    public int TokenExpiryTimeMinutes { get; set; }
    public int RefreshTokenExpiryTimeMonths { get; set; }

    public int DevelopRefreshTokenExpiryTimeMinutes { get; set; }


}