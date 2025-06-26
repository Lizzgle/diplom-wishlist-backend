namespace Identity.Presentation.Models.Login;

public class LoginResponseModel
{
    public string Email { get; set; }
    
    public string JwtToken { get; set; }
    
    public string RefreshToken { get; set; }
}