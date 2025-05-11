using AutoMapper;
using MediatR;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;

public class GetWishlistsByUserIdHandler(IWishlistRepository wishlistRepository, IMapper mapper)
    : IRequestHandler<GetWishlistsByUserIdRequest, GetWishlistsByUserIdResponse>
{
    public async Task<GetWishlistsByUserIdResponse> Handle(GetWishlistsByUserIdRequest request, CancellationToken cancellationToken)
    {
        var wishlists = await wishlistRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
        var wishlistsDto = mapper.Map<List<WishlistDto>>(wishlists);
        
        return new GetWishlistsByUserIdResponse() { Wishlists = wishlistsDto, Count = wishlistsDto.Count};
    }
}