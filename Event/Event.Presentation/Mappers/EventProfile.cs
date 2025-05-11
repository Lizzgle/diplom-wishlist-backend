using AutoMapper;
using Event.Application.Usecases.Events.Commands.Create;
using Event.Application.Usecases.Events.Commands.Update;
using Event.Application.Usecases.Events.Queries.GetAllForUser;
using Event.Application.Usecases.Events.Queries.GetById;
using Event.Presentation.Models.Events;

namespace Event.Presentation.Mappers;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<CreateEventRequestModel, CreateEventRequest>();
        
        CreateMap<UpdateEventRequestModel, UpdateEventRequest>();

        CreateMap<GetByIdResponse, GetEventByIdResponseModel>();
        CreateMap<GetAllForUserResponse, GetEventsResponseModel>();
    }
}