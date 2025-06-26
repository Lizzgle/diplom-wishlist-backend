using MediatR;

namespace Wishlist.Application.Usecases.Wishlists.Commands.Create;

public class CreateWishlistRequest : IRequest
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public required string UserId { get; set; }
}