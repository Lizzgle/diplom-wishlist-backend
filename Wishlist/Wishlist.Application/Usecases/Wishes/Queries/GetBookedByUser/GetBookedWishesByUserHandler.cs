using AutoMapper;
using MediatR;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;

public class GetBookedWishesByUserHandler(IWishRepository wishRepository, IMapper mapper) : IRequestHandler<GetBookedWishesByUserRequest, GetBookedWishesByUserResponse>
{
    public async Task<GetBookedWishesByUserResponse> Handle(GetBookedWishesByUserRequest request, CancellationToken cancellationToken)
    {
        var wishes = await wishRepository.GetBookedWishesAsync(request.UserId, cancellationToken);
        
        //TODO добавть имя создателя
        
        return mapper.Map<GetBookedWishesByUserResponse>(wishes);
    }
}