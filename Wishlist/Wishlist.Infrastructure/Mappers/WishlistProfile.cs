using AutoMapper;
using Wishlist.Infrastructure.Models;

namespace Wishlist.Infrastructure.Mappers;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<Domain.Wishlist, WishlistDto>();
        CreateMap<WishlistDto, Domain.Wishlist>();
    }
}