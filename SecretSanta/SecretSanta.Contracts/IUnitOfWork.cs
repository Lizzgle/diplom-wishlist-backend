using SecretSanta.Contracts.Repositories;

namespace SecretSanta.Contracts;

public interface IUnitOfWork
{
    IGameRepository GameRepository { get; }

    IPlayerRepository PlayerRepository { get; }
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}