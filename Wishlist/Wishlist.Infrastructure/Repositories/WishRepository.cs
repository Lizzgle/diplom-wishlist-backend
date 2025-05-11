using AutoMapper;
using MongoDB.Driver;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;
using Wishlist.Domain.Enums;
using Wishlist.Infrastructure.Models;

namespace Wishlist.Infrastructure.Repositories;

public class WishRepository(IMongoDatabase database, IMapper mapper) : IWishRepository
{
    private readonly IMongoCollection<WishlistDto> _wishlistCollection = database.GetCollection<WishlistDto>("wishlists");

    public async Task CreateAsync(Wish wish, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, wish.WishlistId);

        var wishDto = mapper.Map<WishDto>(wish);

        var update = Builders<WishlistDto>.Update.Push(w => w.Wishes, wishDto);

        await _wishlistCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Wish wish, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.And(
            Builders<WishlistDto>.Filter.Eq(w => w.Id, wish.WishlistId),
            Builders<WishlistDto>.Filter.ElemMatch(w => w.Wishes, w => w.Id == wish.Id) 
        );

        var wishDto = mapper.Map<WishDto>(wish);

        var update = Builders<WishlistDto>.Update
            .Set("Wishes.$", wishDto); 

        await _wishlistCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid id, Guid wishlistId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, wishlistId);

        var update = Builders<WishlistDto>.Update.PullFilter(w => w.Wishes, w => w.Id == id);

        await _wishlistCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<Wish> GetByIdAsync(Guid id, Guid wishlistId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, wishlistId);

        var wishlistDto = await _wishlistCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);

        var wishDto = wishlistDto.Wishes.FirstOrDefault(w => w.Id == id);

        return mapper.Map<Wish>(wishDto);
    }

    public async Task<bool> IsWishInWishlistExist(string name, Guid wishlistId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.Eq(w => w.Id, wishlistId);

        var wishlistDto = await _wishlistCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return wishlistDto.Wishes.Any(w => w.Name == name);
    }

    public async Task<List<Wish>> GetBookedWishesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<WishlistDto>.Filter.ElemMatch(w => w.Wishes, wish => wish.BookedBy == userId && wish.Status == WishStatus.Available);

        var wishlistDtos = await _wishlistCollection.Find(filter).ToListAsync(cancellationToken);

        var bookedWishes = wishlistDtos
            .SelectMany(w => w.Wishes)
            .Where(w => w.BookedBy == userId && w.Status == WishStatus.Available)
            .ToList();

        return mapper.Map<List<Wish>>(bookedWishes);
    }
}