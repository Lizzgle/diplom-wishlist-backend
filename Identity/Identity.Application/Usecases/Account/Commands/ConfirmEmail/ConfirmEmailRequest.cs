using MediatR;

namespace Identity.Application.Usecases.Account.Commands.ConfirmEmail;

public class ConfirmEmailRequest : IRequest
{
    public string? Email { get; set; }
    
    public string? Code { get; set; }
}