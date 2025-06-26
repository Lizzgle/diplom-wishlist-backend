namespace Wishlist.Contracts.Repositories;

public interface IWishlistRepository
{
    Task CreateAsync(Domain.Wishlist wishlist, CancellationToken cancellationToken = default);

    Task UpdateAsync(Domain.Wishlist wishlist, CancellationToken cancellationToken = default);

    Task DeleteAsync(Domain.Wishlist wishlist, CancellationToken cancellationToken = default);

    Task DeleteRangeByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    Task<Domain.Wishlist> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Domain.Wishlist>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<bool> IsExistsByNameAsync(string name, string userId, CancellationToken cancellationToken = default);
}