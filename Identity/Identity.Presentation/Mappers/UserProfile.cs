using AutoMapper;
using Identity.Application.Usecases.Account.Commands.Login;
using Identity.Application.Usecases.Account.Commands.Registration;
using Identity.Application.Usecases.Users.Queries.GetUserInfo;
using Identity.Application.Usecases.Users.Queries.GetUsersNames;
using Identity.Contracts.Models;
using Identity.Presentation.Models;
using Identity.Presentation.Models.GetUserInfo;
using Identity.Presentation.Models.Login;
using Identity.Presentation.Models.Register;

namespace Identity.Presentation.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterRequest, RegistrationRequest>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword))
            .ForMember(dst => dst.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
        
        CreateMap<RegistrationResponse, RegisterResponseModel>();

        CreateMap<LoginRequestModel, LoginRequest>();
        CreateMap<LoginResponse, LoginResponseModel>();
        
        CreateMap<GetUserInfoResponse, GetUserInfoResponseModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
        
        CreateMap<GetUsersNamesResponse, List<GetUsersNamesResponseModel>>()
            .ConvertUsing(src => src.Users.Select(user => new GetUsersNamesResponseModel
            {
                UserId = user.Id,
                UserName = user.UserName
            }).ToList());

        CreateMap<GetUsersNamesByIdsDto, GetUsersNamesResponseModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));

    }
}