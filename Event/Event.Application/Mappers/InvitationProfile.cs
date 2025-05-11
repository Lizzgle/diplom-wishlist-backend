using AutoMapper;
using Event.Application.Usecases.Invitations.Queries.GetByEventId;
using Event.Application.Usecases.Invitations.Queries.GetById;
using Event.Application.Usecases.Invitations.Queries.GetByUserId;
using Event.Domain;

namespace Event.Application.Mappers;

public class InvitationProfile : Profile
{
    public InvitationProfile()
    {
        CreateMap<Invitation, GetByIdResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.InvitedId, opt => opt.MapFrom(src => src.InvitedId))
            .ForMember(dest => dest.InvitedEmail, otp => otp.MapFrom(src => src.Invited.Email))
            .ForMember(dest => dest.OrganizerId, opt => opt.MapFrom(src => src.OrganizerId))
            .ForMember(dest => dest.OrganizerEmail, otp => otp.MapFrom(src => src.Organizer.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
            .ForMember(dest => dest.EventName, otp => otp.MapFrom(src => src.Event.Name))
            .ForMember(dest => dest.EventDate, otp => otp.MapFrom(src => src.Event.DateOfEvent));

        CreateMap<Invitation, InvitationByEventId>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.InvitedId, opt => opt.MapFrom(src => src.InvitedId))
            .ForMember(dest => dest.InvitedEmail, otp => otp.MapFrom(src => src.Invited.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap<Invitation, InvitationByUserId>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrganizerId, opt => opt.MapFrom(src => src.OrganizerId))
            .ForMember(dest => dest.OrganizerEmail, otp => otp.MapFrom(src => src.Organizer.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
            .ForMember(dest => dest.EventName, otp => otp.MapFrom(src => src.Event.Name))
            .ForMember(dest => dest.EventDate, otp => otp.MapFrom(src => src.Event.DateOfEvent));
    }
}