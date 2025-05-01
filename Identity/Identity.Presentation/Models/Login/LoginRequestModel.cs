namespace Identity.Presentation.Models.Login;

public class LoginRequestModel
{
    public required string UniqueProperty { get; set; }
    
    public required string Password { get; set; }
}