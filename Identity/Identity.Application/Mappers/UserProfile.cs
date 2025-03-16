using AutoMapper;
using Identity.Application.Usecases.Users.Commands.Registration;
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
    }
}