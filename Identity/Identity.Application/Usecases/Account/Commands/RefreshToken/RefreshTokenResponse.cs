namespace Identity.Application.Usecases.Account.Commands.RefreshToken;

public class RefreshTokenResponse
{
    public string Email { get; set; }
    
    public string JwtToken { get; set; }
}