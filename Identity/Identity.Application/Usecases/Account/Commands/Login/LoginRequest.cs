using Common.Models;
using MediatR;

namespace Identity.Application.Usecases.Account.Commands.Login;

public class LoginRequest : IRequest<LoginResponse>
{
    public required string UniqueProperty { get; set; }
    
    public required string Password { get; set; }
    
    public required DeviceType DeviceType { get; set; }
}