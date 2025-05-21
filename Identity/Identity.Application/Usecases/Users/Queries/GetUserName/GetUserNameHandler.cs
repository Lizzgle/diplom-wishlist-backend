using Core.Exceptions;
using Identity.Contracts.Repositories;
using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUserName;

public class GetUserNameHandler(IUserRepository userRepository) : IRequestHandler<GetUserNameRequest, string>
{
    public async Task<string> Handle(GetUserNameRequest request, CancellationToken cancellationToken)
    {
        var username = await userRepository.GetUsernameByIdAsync(request.UserId, cancellationToken);
        if (username is null)
            throw new NotFoundException();
        
        return username;
    }
}