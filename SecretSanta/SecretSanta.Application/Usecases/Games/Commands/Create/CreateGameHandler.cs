using AutoMapper;
using MediatR;
using SecretSanta.Contracts;
using SecretSanta.Contracts.Providers;
using SecretSanta.Domain;

namespace SecretSanta.Application.Usecases.Games.Commands.Create;

public class CreateGameHandler(IUnitOfWork unitOfWork, IMapper mapper, IUrlProvider urlProvider) : IRequestHandler<CreateGameRequest>
{
    public async Task Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        var game = mapper.Map<Game>(request);
        game.Url = urlProvider.GenerateUrl(request.CreatorId, game.Id);
        
        await unitOfWork.GameRepository.AddAsync(game, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}