using AutoMapper;
using Core.Exceptions;
using MediatR;
using SecretSanta.Contracts;
using SecretSanta.Domain;

namespace SecretSanta.Application.Usecases.Players.Queries.GetById;

public class GetPlayerByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<GetPlayerByIdRequest, GetPlayerByIdResponse>
{
    public async Task<GetPlayerByIdResponse> Handle(GetPlayerByIdRequest request, CancellationToken cancellationToken)
    {
        var player = await unitOfWork.PlayerRepository.GetByIdAsync(request.PlayerId, cancellationToken);
        if (player is null)
            throw new NotFoundException("Player not found");

        var playerDto = mapper.Map<GetPlayerByIdResponse>(player);
        
        if (request.UserId == player.UserId)
            return playerDto;

        playerDto.RecipientId = null;
        
        var user = await unitOfWork.PlayerRepository.GetPlayerByGameAndUserAsync(player.GameId, request.UserId, cancellationToken);
        if (user is null)
           throw new NotFoundException("User not found");

        if (player.RecipientId != user.Id)
           throw new ForbiddenException(); 
        
        return playerDto;
    }
}