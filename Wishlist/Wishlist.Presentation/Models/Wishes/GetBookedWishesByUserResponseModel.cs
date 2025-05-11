using Wishlist.Domain;

namespace Wishlist.Presentation.Models.Wishes;

public class GetBookedWishesByUserResponseModel
{
    public List<BookedWishModel> BookedWishes { get; set; }
}

public class BookedWishModel
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