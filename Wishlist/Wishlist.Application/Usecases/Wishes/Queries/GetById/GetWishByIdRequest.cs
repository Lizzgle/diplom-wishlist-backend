using MediatR;

namespace Wishlist.Application.Usecases.Wishes.Queries.GetById;

public class GetWishByIdRequest : IRequest<GetWishByIdResponse>
{
    public Guid Id { get; set; }
    
    public Guid WishlistId { get; set; }
}