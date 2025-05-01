namespace Identity.Presentation.Models.RefreshToken;

public class RefreshTokenResponseModel
{
    public string Email { get; set; }
    
    public string JwtToken { get; set; }
}