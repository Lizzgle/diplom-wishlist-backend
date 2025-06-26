using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUserName;

public class GetUserNameRequest : IRequest<string>
{
    public required string UserId { get; set; }
}