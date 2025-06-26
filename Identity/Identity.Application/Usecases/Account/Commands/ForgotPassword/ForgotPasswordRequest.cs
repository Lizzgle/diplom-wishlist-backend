using MediatR;

namespace Identity.Application.Usecases.Account.Commands.ForgotPassword;

public class ForgotPasswordRequest : IRequest<ForgotPasswordResponse>
{
    public required string Email { get; set; }
}