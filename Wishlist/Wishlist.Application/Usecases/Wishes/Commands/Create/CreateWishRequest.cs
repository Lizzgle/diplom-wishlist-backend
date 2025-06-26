using MediatR;
using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.Application.Usecases.Wishes.Commands.Create;

public class CreateWishRequest : IRequest
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public WishStatus Status { get; set; }

    public int Rating { get; set; }
    
    public List<Link> Links { get; set; } = new List<Link>();
    
    public FileData? File { get; set; }
    
    public Stream? FileStream { get; init; }
    
    public Guid WishlistId { get; set; }
    
    public string CreatorId { get; set; }
}