using MediatR;

namespace Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;

public class GetBookedWishesByUserRequest : IRequest<GetBookedWishesByUserResponse>
{
    public required string UserId { get; set; }
}