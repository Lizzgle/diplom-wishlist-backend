namespace Identity.Presentation.Models.ForgotPassword;

public class ForgotPasswordResponseModel
{
    public required string Email { get; set; }
    
    public required string Url { get; set; }
}