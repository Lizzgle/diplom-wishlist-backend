using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;

public class GetBookedWishesByUserResponse
{
    public List<BookedWish> BookedWishes { get; set; }
}

public class BookedWish
{
    public Guid WishId { get; init; }
    
    public string Name { get; init; }
    
    public string Description { get; init; }
    
    public decimal Price { get; init; }
    
    public FileData? File { get; init; }
    
    public List<Link> Links { get; init; }
    
    public Guid WishlistId { get; init; }
    
    public required string CreatorId { get; init; }
    
    public required string CreatorName { get; init; }
}