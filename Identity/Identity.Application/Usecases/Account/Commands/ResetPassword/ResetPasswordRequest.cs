using MediatR;

namespace Identity.Application.Usecases.Account.Commands.ResetPassword;

public class ResetPasswordRequest : IRequest
{
    public string? Email { get; set; }
    
    public string? Code { get; set; }
    
    public required string Password { get; set; }
    
    public required string ConfirmPassword { get; set; }
}