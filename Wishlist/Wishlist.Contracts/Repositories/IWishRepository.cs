using Wishlist.Domain;

namespace Wishlist.Contracts.Repositories;

public interface IWishRepository
{
    Task CreateAsync(Wish wish, CancellationToken cancellationToken = default);

    Task UpdateAsync(Wish wish, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, Guid wishlistId, CancellationToken cancellationToken = default);

    Task<Wish> GetByIdAsync(Guid id, Guid wishlistId, CancellationToken cancellationToken = default);

    Task<bool> IsWishInWishlistExist(string name, Guid wishlistId, CancellationToken cancellationToken = default);
    
    //TODO сделать фильтрацию по статусу
    Task<List<Wish>> GetBookedWishesAsync(string userId, CancellationToken cancellationToken = default);
}