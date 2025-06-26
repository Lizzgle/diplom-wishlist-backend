using AutoMapper;
using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishes.Queries.GetById;

public class GetWishByIdHandler(IWishRepository wishRepository, IMapper mapper) : IRequestHandler<GetWishByIdRequest, GetWishByIdResponse>
{
    public async Task<GetWishByIdResponse> Handle(GetWishByIdRequest request, CancellationToken cancellationToken)
    {
        var wish = await wishRepository.GetByIdAsync(request.Id, request.WishlistId, cancellationToken);
        if (wish is null)
            throw new NotFoundException("Wish doesn't exist");
        
        return mapper.Map<GetWishByIdResponse>(wish);
    }
}