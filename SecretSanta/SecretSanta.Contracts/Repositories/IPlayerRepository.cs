using SecretSanta.Domain;

namespace SecretSanta.Contracts.Repositories;

public interface IPlayerRepository : IBaseRepository<Player>
{
    Task<List<Player>> GetAllPlayersByGameAsync(Guid gameId, CancellationToken cancellationToken = default);
}