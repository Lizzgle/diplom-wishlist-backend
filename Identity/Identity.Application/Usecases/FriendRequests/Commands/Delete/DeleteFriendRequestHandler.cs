using Core.Exceptions;
using Identity.Contracts.Repositories;
using MediatR;

namespace Identity.Application.Usecases.FriendRequests.Commands.Delete;

public class DeleteFriendRequestHandler(IFriendRequestRepository friendRequestRepository) : IRequestHandler<DeleteFriendRequest>
{
    public async Task Handle(DeleteFriendRequest request, CancellationToken cancellationToken)
    {
        var friendRequest = await friendRequestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (friendRequest is null)
            throw new NotFoundException("FriendRequest not found");
        
        if (friendRequest.SenderId != request.SenderId)
            throw new ForbiddenException("You do not have access to this request");
        
        await friendRequestRepository.DeleteAsync(friendRequest, cancellationToken);
    }
}