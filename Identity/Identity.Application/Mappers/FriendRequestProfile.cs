using AutoMapper;
using Identity.Domain;

using ReceivedFriendRequestDto = Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests.FriendRequestDto;
using SentFriendRequestDto = Identity.Application.Usecases.FriendRequests.Queries.GetSentFriendRequests.FriendRequestDto;

namespace Identity.Application.Mappers;

public class FriendRequestProfile : Profile
{
    public FriendRequestProfile()
    {
        CreateMap<FriendRequest, ReceivedFriendRequestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
            .ForMember(dest => dest.SenderEmail, opt => opt.MapFrom(src => src.Sender.Email))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName));
        
        CreateMap<FriendRequest, SentFriendRequestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
            .ForMember(dest => dest.ReceiverEmail, opt => opt.MapFrom(src => src.Receiver.Email))
            .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.UserName));
    }
}