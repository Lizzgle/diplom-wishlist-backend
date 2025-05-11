using MediatR;

namespace Wishlist.Application.Usecases.Wishlists.Queries.GetById;

public class GetWishlistByIdRequest : IRequest<GetWishlistByIdResponse>
{
    public Guid Id { get; set; }
    
    public required string UserId { get; set; }
}