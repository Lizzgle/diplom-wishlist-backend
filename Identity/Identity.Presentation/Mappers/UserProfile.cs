using AutoMapper;
using Identity.Application.Usecases.Account.Commands.Login;
using Identity.Application.Usecases.Account.Commands.Registration;
using Identity.Presentation.Models.Login;
using Identity.Presentation.Models.Register;

namespace Identity.Presentation.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterRequest, RegistrationRequest>();
        CreateMap<RegistrationResponse, RegisterResponseModel>();

        CreateMap<LoginRequestModel, LoginRequest>();
        CreateMap<LoginResponse, LoginResponseModel>();
    }
}