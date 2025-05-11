using AutoMapper;
using Wishlist.Domain;
using Wishlist.Infrastructure.Models;

namespace Wishlist.Infrastructure.Mappers;

public class WishProfile : Profile
{
    public WishProfile()
    {
        CreateMap<Wish, WishDto>();
        CreateMap<WishDto, Wish>();
    }
    
}