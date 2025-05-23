using AutoMapper;
using SecretSanta.Application.Usecases.Games.Commands.Create;
using SecretSanta.Application.Usecases.Games.Queries.GetAllForUser;
using SecretSanta.Application.Usecases.Games.Queries.GetById;
using SecretSanta.Presentation.Models.Games;

namespace SecretSanta.Presentation.Mappers;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<CreateGameRequestModel, CreateGameRequest>();

        CreateMap<GetAllGamesForUserResponse, GetAllGamesForUserResponseModel>();
        CreateMap<GameDto, GameDtoModel>();

        CreateMap<GetGameByIdResponse, GetGameByIdResponseModel>();
    }
}