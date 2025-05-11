namespace Wishlist.Presentation.Models;

public class UpdateWishlistRequestModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}