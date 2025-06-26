namespace Wishlist.Presentation.Models;

public class GetWishlistsByUserIdResponseModel
{
    public int Count { get; set; }
    
    public List<WishlistDtoModel> Wishlists { get; set; } = new List<WishlistDtoModel>();
}

public class WishlistDtoModel
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public int CountOfWihes { get; set; }
    
    // TODO добавть иконку
}