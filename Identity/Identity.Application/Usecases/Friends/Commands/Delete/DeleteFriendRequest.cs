using MediatR;

namespace Identity.Application.Usecases.Friends.Commands.Delete;

public class DeleteFriendRequest : IRequest
{
    public required string UserId { get; set; }
    
    public required string FriendId { get; set; }
}