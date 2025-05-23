using SecretSanta.Domain;

namespace SecretSanta.Contracts.Repositories;

public interface IPlayerRepository : IBaseRepository<Player>
{
    Task<List<Player>> GetAllPlayersByGameAsync(Guid gameId, CancellationToken cancellationToken = default);
    
    Task<List<Player>> GetAllPlayersByUserAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<Player?> GetPlayerByGameAndUserAsync(Guid gameId, string userId, CancellationToken cancellationToken = default);
    
    Task UpdateRangeAsync(List<Player> players, CancellationToken cancellationToken = default);
}