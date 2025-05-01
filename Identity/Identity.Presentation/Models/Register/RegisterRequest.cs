namespace Identity.Presentation.Models.Register;

public class RegisterRequest
{
    public required string UserName { get; set; }

    public required string Email { get; set; }
    
    public required DateTime DateOfBirth { get; set; }

    public required string Password { get; set; }

    public required string ConfirmPassword { get; set; }
}