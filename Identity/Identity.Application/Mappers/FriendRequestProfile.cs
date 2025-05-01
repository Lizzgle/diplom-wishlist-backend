using AutoMapper;
using Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;
using Identity.Domain;

namespace Identity.Application.Mappers;

public class FriendRequestProfile : Profile
{
    public FriendRequestProfile()
    {
        CreateMap<FriendRequest, FriendRequestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
            .ForMember(dest => dest.SenderEmail, opt => opt.MapFrom(src => src.Sender.Email))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName));
    }
}