using Microsoft.EntityFrameworkCore;
using SecretSanta.Contracts.Repositories;
using SecretSanta.Domain;

namespace SecretSanta.Infrastructure.Repositories;

public class GameRepository(AppDbContext context) : BaseRepository<Game>(context), IGameRepository
{
    public async Task<List<Game>> GetGamesByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await context.Games
            .AsNoTracking()
            .Where(g => ids.Contains(g.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Game?> GetGameByIdWithIncludeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Games
            .AsNoTracking()
            .Where(g => id == g.Id)
            .Include(g => g.Players)
            .FirstOrDefaultAsync(cancellationToken);
    }
}