using MediatR;

namespace Identity.Application.Usecases.Friends.Queries.GetFriends;

public class GetFriendsRequest : IRequest<GetFriendsResponse>
{
    public required string UserId { get; set; }
}