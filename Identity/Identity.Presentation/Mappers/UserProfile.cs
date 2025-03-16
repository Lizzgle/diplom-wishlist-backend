using AutoMapper;
using Identity.Application.Usecases.Users.Commands.Registration;
using Identity.Presentation.Models.Register;

namespace Identity.Presentation.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterRequest, RegistrationRequest>();
        CreateMap<RegistrationResponse, RegisterResponse>();
    }
}