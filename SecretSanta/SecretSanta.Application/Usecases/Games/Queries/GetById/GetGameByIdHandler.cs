using AutoMapper;
using Core.Exceptions;
using MediatR;
using SecretSanta.Contracts;

namespace SecretSanta.Application.Usecases.Games.Queries.GetById;

public class GetGameByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetGameByIdRequest, GetGameByIdResponse>
{
    public async Task<GetGameByIdResponse> Handle(GetGameByIdRequest request, CancellationToken cancellationToken)
    {
        var game = await unitOfWork.GameRepository.GetGameByIdWithIncludeAsync(request.GameId, cancellationToken);
        if (game is null)
            throw new NotFoundException("Game not found");
        
        return mapper.Map<GetGameByIdResponse>(game);
    }
}