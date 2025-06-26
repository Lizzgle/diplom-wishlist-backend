using MediatR;

namespace Wishlist.Application.Usecases.Wishlists.Commands.Update;

public class UpdateWishlistRequest : IRequest
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string UserId { get; set; }
}