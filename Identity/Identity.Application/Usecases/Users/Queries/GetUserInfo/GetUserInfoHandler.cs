using AutoMapper;
using Core.Exceptions;
using Identity.Contracts.Repositories;
using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUserInfo;

public class GetUserInfoHandler(IUserRepository userRepository, IMapper mapper) 
    : IRequestHandler<GetUserInfoRequest, GetUserInfoResponse>
{
    public async Task<GetUserInfoResponse> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.Id, cancellationToken);
        if (user is null)
            throw new NotFoundException($"User with id = {request.Id} not found while {nameof(GetUserInfoHandler)}");
        
        return mapper.Map<GetUserInfoResponse>(user);
    }
}