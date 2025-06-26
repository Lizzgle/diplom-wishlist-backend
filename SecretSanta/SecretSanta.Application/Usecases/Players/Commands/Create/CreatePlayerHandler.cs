using AutoMapper;
using Core.Exceptions;
using MediatR;
using SecretSanta.Contracts;
using SecretSanta.Domain;

namespace SecretSanta.Application.Usecases.Players.Commands.Create;

public class CreatePlayerHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreatePlayerRequest>
{
    public async Task Handle(CreatePlayerRequest request, CancellationToken cancellationToken)
    {
        var participant = await unitOfWork.PlayerRepository.GetPlayerByGameAndUserAsync(request.GameId, request.UserId, cancellationToken);
        if (participant is not null)
            throw new AlreadyExistException("Player is already exists");
        
        var newParticipant = mapper.Map<Player>(request);
        
        await unitOfWork.PlayerRepository.AddAsync(newParticipant, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}