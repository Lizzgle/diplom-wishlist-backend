using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Commands.Delete;

public class DeleteFriendRequest : IRequest
{
    public required Guid Id { get; set; }
    
    public required string SenderId { get; set; }
}