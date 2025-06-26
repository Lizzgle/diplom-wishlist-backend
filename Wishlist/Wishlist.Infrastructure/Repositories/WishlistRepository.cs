using AutoMapper;
using MongoDB.Driver;
using Wishlist.Contracts.Repositories;
using Wishlist.Infrastructure.Models;

namespace Wishlist.Infrastructure.Repositories;

public class WishlistRepository(IMongoDatabase database, IMapper mapper) : IWishlistRepository
{
    private readonly IMongoCollection<WishlistDto> _wishlistCollection = database.GetCollection<WishlistDto>("wishlists");

    public async Task CreateAsync(Domain.Wishlist wishlist, CancellationToken cancellationToken = default)
    {
        var wishlistDto = mapper.Map<WishlistDto>(wishlist);

        await _wishlistCollection.InsertOneAsync(wishlistDto, cancellationToken: cancellationToken);
    }

    public Task UpdateAsync(Domain.Wishlist wishlist, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, wishlist.Id);

        var update = Builders<WishlistDto>.Update
            .Set(w => w.Name, wishlist.Name)
            .Set(w => w.Description, wishlist.Description);

        return _wishlistCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Domain.Wishlist wishlist, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, wishlist.Id);

        await _wishlistCollection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task DeleteRangeByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.CreatorId, userId);

        await _wishlistCollection.DeleteManyAsync(filter, cancellationToken);
    }

    public async Task<Domain.Wishlist> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, id);

        var wishlistDto = await _wishlistCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return mapper.Map<Domain.Wishlist>(wishlistDto);
    }

    public async Task<List<Domain.Wishlist>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.CreatorId, userId);

        var wishlists = await _wishlistCollection.Find(filter).ToListAsync(cancellationToken);

        return mapper.Map<List<Domain.Wishlist>>(wishlists);
    }

    public async Task<bool> IsExistsByNameAsync(string name, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.CreatorId, userId);

        var wishlists = await _wishlistCollection.Find(filter).ToListAsync(cancellationToken);

        return wishlists.Any(w => w.Name == name);
    }
}