using MediatR;

namespace SecretSanta.Application.Usecases.Games.Commands.Delete;

public class DeleteGameRequest : IRequest
{
    public required string UserId { get; set; }
    
    public Guid GameId { get; set; }
}