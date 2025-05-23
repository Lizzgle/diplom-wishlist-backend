using AutoMapper;
using SecretSanta.Application.Usecases.Players.Commands.Create;
using SecretSanta.Application.Usecases.Players.Queries.GetById;
using SecretSanta.Presentation.Models.Players;

namespace SecretSanta.Presentation.Mappers;

public class PlayerProfile : Profile
{
    public PlayerProfile()
    {
        CreateMap<CreatePlayerRequestModel, CreatePlayerRequest>()
            .ForMember(dest => dest.Card, opt => opt.MapFrom(src => src.Card))
            .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId));
        
        CreateMap<GetPlayerByIdResponse, GetPlayerByIdResponseModel>()
            .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.PlayerId))
            .ForMember(dest => dest.Card, opt => opt.MapFrom(src => src.Card))
            .ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.RecipientId));
    }
}