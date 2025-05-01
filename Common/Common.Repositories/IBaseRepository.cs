namespace Common.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    public Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    public Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}