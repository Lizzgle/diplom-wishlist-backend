namespace Identity.Application.Usecases.Account.Commands.Registration;

public class RegistrationResponse
{
    public required string Email { get; set; }
    
    public required string Code { get; set; }
}