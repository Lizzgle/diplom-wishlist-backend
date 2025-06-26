using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUserInfo;

public class GetUserInfoRequest : IRequest<GetUserInfoResponse>
{
    public required string Id { get; set; }
}