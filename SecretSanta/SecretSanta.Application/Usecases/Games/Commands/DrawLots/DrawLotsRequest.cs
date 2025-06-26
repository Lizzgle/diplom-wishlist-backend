using MediatR;

namespace SecretSanta.Application.Usecases.Games.Commands.DrawLots;

public class DrawLotsRequest : IRequest
{
    public required string UserId { get; set; }
    
    public Guid GameId { get; set; }
}