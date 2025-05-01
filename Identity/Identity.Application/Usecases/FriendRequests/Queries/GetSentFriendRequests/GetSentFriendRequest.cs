using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Queries.GetSentFriendRequests;

public class GetSentFriendRequest : IRequest<GetSentFriendRequestsResponse>
{
    public required string UserId { get; set; }
}