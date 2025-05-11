using AutoMapper;
using Wishlist.Application.Usecases.Wishes.Commands.Create;
using Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;
using Wishlist.Application.Usecases.Wishes.Queries.GetById;
using Wishlist.Application.Usecases.Wishlists.Queries.GetById;
using Wishlist.Domain;

namespace Wishlist.Application.Mappers;

public class WishProfile : Profile
{
    public WishProfile()
    {
        CreateMap<Wish, WishDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.IsBooked, opt => opt.MapFrom(src => src.IsBooked))
            .ForMember(dest => dest.BookedBy, opt => opt.MapFrom(src => src.BookedBy))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Links.Count != 0
                ? src.Links.OrderBy(l => l.Price).First().Price 
                : 0));
        
        CreateMap<CreateWishRequest, Wish>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File))
            .ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.Links))
            .ForMember(dest => dest.WishlistId, opt => opt.MapFrom(src => src.WishlistId));
            
        CreateMap<Wish, BookedWish>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File))
            .ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.Links))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Links.Count != 0
                ? src.Links.OrderBy(l => l.Price).First().Price 
                : 0));

        CreateMap<Wish, GetWishByIdResponse>();
    }
}