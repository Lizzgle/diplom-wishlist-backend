using Core.Models;
using MediatR;

namespace Identity.Application.Usecases.Account.Commands.RefreshToken;

public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
{
    public required string RefreshToken { get; set; }
    
    public required string Id { get; set; }
    
    public required DeviceType DeviceType { get; set; }
}