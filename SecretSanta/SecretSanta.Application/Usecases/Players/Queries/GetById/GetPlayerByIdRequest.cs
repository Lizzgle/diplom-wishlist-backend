using MediatR;

namespace SecretSanta.Application.Usecases.Players.Queries.GetById;

public class GetPlayerByIdRequest : IRequest<GetPlayerByIdResponse>
{
    public required string UserId { get; set; }
    
    public Guid PlayerId { get; set; }
}