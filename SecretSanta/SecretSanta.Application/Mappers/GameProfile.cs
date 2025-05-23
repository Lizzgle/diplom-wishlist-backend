using AutoMapper;
using SecretSanta.Application.Usecases.Games.Commands.Create;
using SecretSanta.Application.Usecases.Games.Queries.GetAllForUser;
using SecretSanta.Application.Usecases.Games.Queries.GetById;
using SecretSanta.Domain;

namespace SecretSanta.Application.Mappers;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<Game, GameDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.DateOfGeneration, opt => opt.MapFrom(src => src.DateOfGeneration));

        CreateMap<CreateGameRequest, Game>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.DateOfGeneration, opt => opt.MapFrom(src => src.DateOfGeneration))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.MinAdvance, opt => opt.MapFrom(src => src.MinAdvance))
            .ForMember(dest => dest.MaxAdvance, opt => opt.MapFrom(src => src.MaxAdvance));
        
        CreateMap<Game, GetGameByIdResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.DateOfGeneration, opt => opt.MapFrom(src => src.DateOfGeneration))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.MinAdvance, opt => opt.MapFrom(src => src.MinAdvance))
            .ForMember(dest => dest.MaxAdvance, opt => opt.MapFrom(src => src.MaxAdvance));
    }
}