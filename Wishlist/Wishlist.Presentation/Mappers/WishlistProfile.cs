using AutoMapper;
using Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;
using Wishlist.Application.Usecases.Wishlists.Commands.Create;
using Wishlist.Application.Usecases.Wishlists.Commands.Update;
using Wishlist.Application.Usecases.Wishlists.Queries.GetById;
using Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;
using Wishlist.Presentation.Models;
using Wishlist.Presentation.Models.Wishlists;

namespace Wishlist.Presentation.Mappers;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<CreateWishlistRequestModel, CreateWishlistRequest>();
        
        CreateMap<UpdateWishlistRequestModel, UpdateWishlistRequest>();
        
        CreateMap<WishlistDto, WishlistDtoModel>();
        CreateMap<GetWishlistsByUserIdResponse, GetWishlistsByUserIdResponseModel>();

        CreateMap<GetWishlistByIdResponse, GetWishlistByIdResponseModel>();
        CreateMap<WishDto, WishDtoModel>();
    }
}