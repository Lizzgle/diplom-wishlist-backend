using AutoMapper;
using Identity.Application.Usecases.Account.Commands.Registration;
using Identity.Application.Usecases.Friends.Queries.GetFriends;
using Identity.Application.Usecases.Users.Queries.GetAllUsers;
using Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;
using Identity.Application.Usecases.Users.Queries.GetUserInfo;
using Identity.Domain;

namespace Identity.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegistrationRequest, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToLower()))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToLower()));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
        
        CreateMap<User, FriendDto>()
            .ForMember(dext => dext.FriendId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dext => dext.FriendEmail, opt => opt.MapFrom(src => src.Email))
            .ForMember(dext => dext.FriendName, opt => opt.MapFrom(src => src.UserName));

        CreateMap<User, GetUserInfoResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

        CreateMap<User, AllUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
    }
}