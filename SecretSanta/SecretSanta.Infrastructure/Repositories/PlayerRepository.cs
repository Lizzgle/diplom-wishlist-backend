using Microsoft.EntityFrameworkCore;
using SecretSanta.Contracts.Repositories;
using SecretSanta.Domain;

namespace SecretSanta.Infrastructure.Repositories;

public class PlayerRepository(AppDbContext context) : BaseRepository<Player>(context), IPlayerRepository
{
    public async Task<List<Player>> GetAllPlayersByGameAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await context.Players
            .AsNoTracking()
            .Where(g => g.GameId == gameId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Player>> GetAllPlayersByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Players
            .AsNoTracking()
            .Where(g => g.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Player?> GetPlayerByGameAndUserAsync(Guid gameId, string userId, CancellationToken cancellationToken = default)
    {
        return await context.Players
            .AsNoTracking()
            .Where(g => g.GameId == gameId && g.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task UpdateRangeAsync(List<Player> players, CancellationToken cancellationToken = default)
    {
        context.Players.UpdateRange(players);
        return Task.CompletedTask;
    }
}