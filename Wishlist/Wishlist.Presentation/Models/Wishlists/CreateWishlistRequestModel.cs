namespace Wishlist.Presentation.Models.Wishlists;

public class CreateWishlistRequestModel
{
    public required string Name { get; init; }

    public string? Description { get; init; }
}