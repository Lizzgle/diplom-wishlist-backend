using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetAllUsers;

public class GetAllUsersRequest : IRequest<GetAllUsersResponse>
{
    public string? UserId { get; set; }
}