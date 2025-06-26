using AutoMapper;
using SecretSanta.Application.Usecases.Players.Commands.Create;
using SecretSanta.Domain;

namespace SecretSanta.Application.Mappers;

public class PlayerProfile : Profile
{
    public PlayerProfile()
    {
        CreateMap<CreatePlayerRequest, Player>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId))
            .ForMember(dest => dest.Card, opt => opt.MapFrom(src => src.Card));
    }
}