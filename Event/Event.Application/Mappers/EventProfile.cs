using AutoMapper;
using Event.Application.Usecases.Events.Commands.Create;
using Event.Application.Usecases.Events.Commands.Update;
using Event.Application.Usecases.Events.Queries.GetAllForUser;
using Event.Application.Usecases.Events.Queries.GetById;

namespace Event.Application.Mappers;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<CreateEventRequest, Domain.Event>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src => src.DateOfEvent))
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => src.Recurrence));
        
        CreateMap<UpdateEventRequest, Domain.Event>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src => src.DateOfEvent))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => src.Recurrence))
            .ForMember(dest => dest.Participants, opt => opt.Ignore());

        CreateMap<Domain.Event, GetByIdResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src => src.DateOfEvent))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => src.Recurrence))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.AccessLevel, opt => opt.MapFrom(src => src.AccessLevel));

        CreateMap<Domain.Event, EventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DateOfEvent, opt => opt.MapFrom(src => src.DateOfEvent));
    }
}