namespace Identity.Presentation.Models;

public class ResetPasswordRequestModel
{
    public required string Password { get; set; }
    
    public required string ConfirmPassword { get; set; }
}