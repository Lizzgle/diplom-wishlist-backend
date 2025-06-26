using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishlists.Commands.Delete;

public class DeleteWishlistHandler(IWishlistRepository wishlistRepository)
    : IRequestHandler<DeleteWishlistRequest>
{
    public async Task Handle(DeleteWishlistRequest request, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (wishlist is null)
            throw new NotFoundException("Wishlist not found");

        if (wishlist.CreatorId != request.UserId)
            throw new ForbiddenException("You cannot delete this wishlist");

        await wishlistRepository.DeleteAsync(wishlist, cancellationToken);
    }
}
