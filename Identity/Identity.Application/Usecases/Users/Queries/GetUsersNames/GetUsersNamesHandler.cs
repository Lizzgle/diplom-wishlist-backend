using Identity.Contracts.Repositories;
using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUsersNames;

public class GetUsersNamesHandler(IUserRepository userRepository) : IRequestHandler<GetUsersNamesRequest, GetUsersNamesResponse>
{
    public async Task<GetUsersNamesResponse> Handle(GetUsersNamesRequest request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersNamesByIdsAsync(request.UserIds, cancellationToken);

        return new GetUsersNamesResponse() { Users = users };
    }
}