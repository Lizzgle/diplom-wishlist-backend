using Microsoft.EntityFrameworkCore;

namespace Common.Repositories;

public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity> 
    where TEntity : class
    where TContext : DbContext
{
    public BaseRepository(TContext dbContext)
    {
        DbContext = dbContext;
    }

    private DbSet<TEntity> Entities => DbContext.Set<TEntity>();
    protected TContext DbContext { get; private set; }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
        
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Entities.Update(entity);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}