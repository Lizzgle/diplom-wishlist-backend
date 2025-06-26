using Core.Exceptions;
using MediatR;
using SecretSanta.Contracts;

namespace SecretSanta.Application.Usecases.Games.Commands.Delete;

public class DeleteGameHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteGameRequest>
{
    public async Task Handle(DeleteGameRequest request, CancellationToken cancellationToken)
    {
        var game = await unitOfWork.GameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            throw new NotFoundException("Game not found");

        if (request.UserId != game.CreatorId)
            throw new ForbiddenException("You cannot delete this game");
        
        await unitOfWork.GameRepository.DeleteAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}