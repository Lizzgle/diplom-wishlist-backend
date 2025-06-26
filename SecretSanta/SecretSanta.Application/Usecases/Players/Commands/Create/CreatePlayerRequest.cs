using MediatR;

namespace SecretSanta.Application.Usecases.Players.Commands.Create;

public class CreatePlayerRequest : IRequest
{
    public required string UserId { get; set; }
    
    public string? Card { get; set; }
    
    public Guid GameId { get; set; }
}