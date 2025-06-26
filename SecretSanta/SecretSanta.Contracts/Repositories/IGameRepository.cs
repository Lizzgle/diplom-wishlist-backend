using SecretSanta.Domain;

namespace SecretSanta.Contracts.Repositories;

public interface IGameRepository : IBaseRepository<Game>
{
    Task<List<Game>> GetGamesByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
    
    Task<Game?> GetGameByIdWithIncludeAsync(Guid id, CancellationToken cancellationToken = default);
}