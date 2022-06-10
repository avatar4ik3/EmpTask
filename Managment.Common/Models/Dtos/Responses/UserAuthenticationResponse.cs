namespace Managment.Common.Models.Dtos.Responses;

public class UserAuthenticationResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}