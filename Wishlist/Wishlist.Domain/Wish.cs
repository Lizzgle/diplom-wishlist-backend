using Wishlist.Domain.Enums;

namespace Wishlist.Domain;

public class Wish
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Name { get; set; }

    public string? Description { get; set; }

    public WishStatus Status { get; set; } = WishStatus.Available;

    public int Rating { get; set; }
    
    public bool IsBooked { get; set; }
    
    public string? BookedBy { get; set; }
    
    public List<Link> Links { get; set; } = new List<Link>();
    
    public FileData File { get; set; }
    
    public Guid WishlistId { get; set; }
    
    public string CreatorId { get; set; }
}