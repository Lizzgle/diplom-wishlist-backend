using MediatR;

namespace SecretSanta.Application.Usecases.Games.Queries.GetById;

public class GetGameByIdRequest : IRequest<GetGameByIdResponse>
{
    public required string UserId { get; set; }
    
    public Guid GameId { get; set; }
}