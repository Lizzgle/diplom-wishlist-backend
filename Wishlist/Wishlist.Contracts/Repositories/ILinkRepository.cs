using Wishlist.Domain;

namespace Wishlist.Contracts.Repositories;

public interface ILinkRepository
{
    Task CreateAsync(Link link, CancellationToken cancellationToken = default);

    Task UpdateAsync(Link link, CancellationToken cancellationToken = default);

    Task DeleteAsync(Link link, Guid wishId, Guid wishlistId, CancellationToken cancellationToken = default);

    Task<Wish> GetByIdAsync(Guid id, Guid wishId, Guid wishlistId, CancellationToken cancellationToken = default);

    Task<bool> IsLinkInWishExist(string name, Guid wishId, CancellationToken cancellationToken = default);
}