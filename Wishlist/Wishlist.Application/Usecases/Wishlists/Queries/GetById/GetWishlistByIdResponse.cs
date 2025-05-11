using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Application.Usecases.Wishlists.Queries.GetById;

public class GetWishlistByIdResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string CreatorId { get; set; }

    public List<WishDto>? Wishes { get; set; }
}

public class WishDto
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public decimal? Price { get; set; }
    
    public WishStatus Status { get; set; }

    public int Rating { get; set; }
    
    public bool IsBooked { get; set; }
    
    public string? BookedBy { get; set; }
    
    public List<Link> Links { get; set; }
    
    public FileData? File { get; set; }
}