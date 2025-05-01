using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Commands.AcceptFriendRequest;

public class AcceptFriendRequest : IRequest
{
    public required Guid Id { get; set; }
    
    public required string ReceiverId { get; set; }
}