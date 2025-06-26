using AutoMapper;
using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishlists.Commands.Create;

public class CreateWishlistHandler(IWishlistRepository wishlistRepository, IMapper mapper, IUrlProvider urlProvider)
    : IRequestHandler<CreateWishlistRequest>
{
    public async Task Handle(CreateWishlistRequest request, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.IsExistsByNameAsync(request.Name, request.UserId, cancellationToken);
        if (wishlist)
            throw new AlreadyExistException($"Wishlist with name = { request.Name } already exists");

        var newWishlist = mapper.Map<Domain.Wishlist>(request);
        newWishlist.Uri = urlProvider.GenerateUrl(request.UserId, newWishlist.Id);
        
        await wishlistRepository.CreateAsync(newWishlist, cancellationToken);
    }
}