using Core.Exceptions;
using MediatR;
using SecretSanta.Contracts;
using SecretSanta.Contracts.Providers;

namespace SecretSanta.Application.Usecases.Games.Commands.DrawLots;

public class DrawLotsHandler(IUnitOfWork unitOfWork, IDrawLotsProvider drawLotsProvider) : IRequestHandler<DrawLotsRequest>
{
    public async Task Handle(DrawLotsRequest request, CancellationToken cancellationToken)
    {
        var game = await unitOfWork.GameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            throw new NotFoundException("Game not found");

        if (request.UserId != game.CreatorId)
            throw new ForbiddenException("You cannot draw this game");
        
        var players = await unitOfWork.PlayerRepository.GetAllPlayersByGameAsync(request.GameId, cancellationToken);
        
        var pairs = drawLotsProvider.DrawLots(players.Select(p => p.Id).ToList());
        
        var updatedPlayers = players.Select(player =>
        {
            player.RecipientId = pairs[player.Id];
            return player;
        }).ToList();
        
        await unitOfWork.PlayerRepository.UpdateRangeAsync(updatedPlayers, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}