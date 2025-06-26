using MediatR;

namespace Wishlist.Application.Usecases.Wishes.Commands.Delete;

public class DeleteWishRequest : IRequest
{ 
    public required Guid WishId { get; init; }

    public required Guid WishlistId { get; init; }

    public required string UserId { get; set; }
}