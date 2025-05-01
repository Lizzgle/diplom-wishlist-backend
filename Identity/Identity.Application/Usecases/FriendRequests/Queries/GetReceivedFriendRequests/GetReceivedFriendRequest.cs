using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;

public class GetReceivedFriendRequest : IRequest<GetReceivedFriendRequestsResponse>
{
    public required string UserId { get; set; }
}