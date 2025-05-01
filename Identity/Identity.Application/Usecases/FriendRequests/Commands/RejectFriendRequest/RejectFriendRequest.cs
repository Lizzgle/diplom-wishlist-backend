using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Commands.RejectFriendRequest;

public class RejectFriendRequest : IRequest
{
    public required Guid Id { get; set; }
    
    public required string ReceiverId { get; set; }
}