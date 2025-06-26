using MediatR;

namespace Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;

public class GetWishlistsByUserIdRequest : IRequest<GetWishlistsByUserIdResponse>
{
    public required string UserId { get; set; }
}