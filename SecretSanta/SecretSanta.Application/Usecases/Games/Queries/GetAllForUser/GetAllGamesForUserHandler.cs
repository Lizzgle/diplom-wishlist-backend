using AutoMapper;
using MediatR;
using SecretSanta.Contracts;

namespace SecretSanta.Application.Usecases.Games.Queries.GetAllForUser;

public class GetAllGamesForUserHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<GetAllGamesForUserRequest, GetAllGamesForUserResponse>
{
    public async Task<GetAllGamesForUserResponse> Handle(GetAllGamesForUserRequest request, CancellationToken cancellationToken)
    {
        var players = await unitOfWork.PlayerRepository.GetAllPlayersByUserAsync(request.UserId, cancellationToken);
        if (players.Count is 0)
            return new GetAllGamesForUserResponse();
        
        var gamesIds = players.Select(p => p.GameId).ToList();
        
        var games = await unitOfWork.GameRepository.GetGamesByIdsAsync(gamesIds, cancellationToken);
        
        var gamesDtos = mapper.Map<List<GameDto>>(games);
        
        return new GetAllGamesForUserResponse() { Games = gamesDtos };
    }
}