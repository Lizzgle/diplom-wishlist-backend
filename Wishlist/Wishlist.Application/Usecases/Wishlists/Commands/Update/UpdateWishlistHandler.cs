using AutoMapper;
using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishlists.Commands.Update;

public class UpdateWishlistHandler(IWishlistRepository wishlistRepository, IMapper mapper)
    : IRequestHandler<UpdateWishlistRequest>
{
    public async Task Handle(UpdateWishlistRequest request, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (wishlist is null)
            throw new NotFoundException("Wishlist not found");

        if (wishlist.CreatorId != request.UserId)
            throw new ForbiddenException("You cannot update this wishlist");

        mapper.Map(request, wishlist);
        await wishlistRepository.UpdateAsync(wishlist, cancellationToken);
    }
}