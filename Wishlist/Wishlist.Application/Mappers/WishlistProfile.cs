using AutoMapper;
using Wishlist.Application.Usecases.Wishlists.Commands.Create;
using Wishlist.Application.Usecases.Wishlists.Commands.Update;
using Wishlist.Application.Usecases.Wishlists.Queries.GetById;
using Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;
using Wishlist.Domain;

namespace Wishlist.Application.Mappers;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<CreateWishlistRequest, Domain.Wishlist>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        
        CreateMap<UpdateWishlistRequest, Domain.Wishlist>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<Domain.Wishlist, GetWishlistByIdResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId));

        CreateMap<Domain.Wishlist, WishlistDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CountOfWihes, otp => otp.MapFrom(src => src.Wishes.Count));
    }
    
}