using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Commands.SendFriendRequest;

public class SendFriendRequest : IRequest
{
    public required string SenderId { get; set; }
    
    public required string ReceiverId { get; set; }
}