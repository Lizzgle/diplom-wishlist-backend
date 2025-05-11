using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Presentation.Models.Wishes;

public class UpdateWishRequestModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public WishStatus Status { get; set; }

    public int Rating { get; set; }
    
    public List<Link> Links { get; set; }
    
    public FileData? File { get; set; }
}