using SecretSanta.Contracts;
using SecretSanta.Contracts.Repositories;
using SecretSanta.Infrastructure.Repositories;

namespace SecretSanta.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    private IGameRepository? _games;
    
    private IPlayerRepository? _players;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IGameRepository GameRepository => _games ??= new GameRepository(_dbContext);
    public IPlayerRepository PlayerRepository => _players ??= new PlayerRepository(_dbContext);
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}