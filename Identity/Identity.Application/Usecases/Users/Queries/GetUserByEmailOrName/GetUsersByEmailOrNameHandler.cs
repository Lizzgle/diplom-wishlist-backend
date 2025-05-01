using AutoMapper;
using Identity.Contracts.Repositories;
using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;

public class GetUsersByEmailOrNameHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetUsersByEmailOrNameRequest, GetUsersByEmailOrNameResponse>
{
    public async Task<GetUsersByEmailOrNameResponse> Handle(GetUsersByEmailOrNameRequest request, 
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersByEmailOrNameAsync(request.Query, cancellationToken);
        
        var userDtos = mapper.Map<List<UserDto>>(users);
        
        return new GetUsersByEmailOrNameResponse { Users = userDtos };
    }
}