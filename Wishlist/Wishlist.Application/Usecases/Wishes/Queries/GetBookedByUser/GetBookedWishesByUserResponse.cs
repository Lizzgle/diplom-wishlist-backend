using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;

public class GetBookedWishesByUserResponse
{
    public List<BookedWish> BookedWishes { get; set; }
}

public class BookedWish
{
    public Guid WishId { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
    
    public FileData? File { get; set; }
    
    public List<Link> Links { get; set; }
    
    public Guid WishlistId { get; set; }
    
    public required string CreatorId { get; set; }
    
    public required string CreatorName { get; set; }
}