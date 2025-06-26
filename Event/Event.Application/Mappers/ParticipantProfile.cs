using AutoMapper;
using Event.Application.Usecases.Events.Queries.GetById;
using Event.Contracts.Models;

namespace Event.Application.Mappers;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<GetUsersNamesResponse, ParticipantDto>();
    }
}