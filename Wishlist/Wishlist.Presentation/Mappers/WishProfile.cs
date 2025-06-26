using AutoMapper;
using Wishlist.Application.Usecases.Wishes.Commands.Create;
using Wishlist.Application.Usecases.Wishes.Commands.Update;
using Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;
using Wishlist.Presentation.Models.Wishes;

namespace Wishlist.Presentation.Mappers;

public class WishProfile : Profile
{
    public WishProfile()
    {
        CreateMap<CreateWishRequestModel, CreateWishRequest>();
        CreateMap<UpdateWishRequestModel, UpdateWishRequest>();

        CreateMap<GetWishByIdResponseModel, GetWishByIdResponseModel>();

        CreateMap<GetBookedWishesByUserResponse, GetBookedWishesByUserResponseModel>();
        CreateMap<BookedWish, BookedWishModel>();
    }
}