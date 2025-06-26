using AutoMapper;
using Event.Application.Usecases.Invitations.Queries.GetByEventId;
using Event.Application.Usecases.Invitations.Queries.GetById;
using Event.Application.Usecases.Invitations.Queries.GetByUserId;
using Event.Presentation.Models.Invitations;

namespace Event.Presentation.Mappers;

public class InvitationProfile : Profile
{
    public InvitationProfile()
    {
        CreateMap<GetByIdResponse, GetByIdResponseModel>();

        CreateMap<InvitationByUserId, InvitationByUserIdModel>();
        CreateMap<GetByUserIdResponse, GetByUserIdResponseModel>();

        CreateMap<InvitationByEventId, InvitationByEventIdModel>();
        CreateMap<GetByEventIdResponse, GetByEventIdResponseModel>();
    }
    
}