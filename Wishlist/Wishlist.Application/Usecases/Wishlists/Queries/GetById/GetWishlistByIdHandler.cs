using AutoMapper;
using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishlists.Queries.GetById;

public class GetWishlistByIdHandler(IWishlistRepository wishlistRepository, IMapper mapper)
    : IRequestHandler<GetWishlistByIdRequest, GetWishlistByIdResponse>
{
    public async Task<GetWishlistByIdResponse> Handle(GetWishlistByIdRequest request, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.GetByIdAsync(request.Id, cancellationToken);
        if (wishlist is null)
            throw new NotFoundException("Wishlist not found");
        
        return mapper.Map<GetWishlistByIdResponse>(wishlist);
    }
}