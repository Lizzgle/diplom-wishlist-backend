using MediatR;

namespace SecretSanta.Application.Usecases.Games.Queries.GetAllForUser;

public class GetAllGamesForUserRequest : IRequest<GetAllGamesForUserResponse>
{
    public required string UserId { get; set; }
}