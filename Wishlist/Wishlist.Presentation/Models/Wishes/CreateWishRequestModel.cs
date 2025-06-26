using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Presentation.Models.Wishes;

public class CreateWishRequestModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public WishStatus Status { get; set; }

    public int Rating { get; set; }
    
    public List<Link> Links { get; set; } = new List<Link>();
    
    public FileData? File { get; set; }
}