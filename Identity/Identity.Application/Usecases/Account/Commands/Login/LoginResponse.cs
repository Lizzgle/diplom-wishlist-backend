namespace Identity.Application.Usecases.Account.Commands.Login;

public class LoginResponse
{
    public string Email { get; set; }
    
    public string JwtToken { get; set; }
    
    public string RefreshToken { get; set; }
}