using MediatR;

namespace Wishlist.Application.Usecases.Wishlists.Commands.Delete;

public class DeleteWishlistRequest : IRequest
{
    public Guid Id { get; set; }
    
    public required string UserId { get; set; }
}