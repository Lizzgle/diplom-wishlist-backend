namespace Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;

public class GetWishlistsByUserIdResponse
{
    public int Count { get; set; }
    
    public List<WishlistDto> Wishlists { get; set; } = new List<WishlistDto>();
}

public class WishlistDto
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public int CountOfWihes { get; set; }
    
    // TODO добавть иконку
}