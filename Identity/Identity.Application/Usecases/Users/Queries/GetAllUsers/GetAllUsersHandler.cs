using AutoMapper;
using Identity.Contracts.Repositories;
using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetAllUsers;

public class GetAllUsersHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetAllUsersRequest, GetAllUsersResponse>
{
    public async Task<GetAllUsersResponse> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllUsersAsync(cancellationToken);
        
        if (request.UserId is not null)
            users = users.Where(user => user.Id != request.UserId).ToList();
        
        return new GetAllUsersResponse() {Users = mapper.Map<List<AllUserDto>>(users)};
    }
}