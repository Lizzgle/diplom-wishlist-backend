using Core.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Usecases.Friends.Commands.Delete;

public class DeleteFriendHandler(IFriendshipRepository friendshipRepository, IFriendRequestRepository friendRequestRepository, IUserRepository userRepository) 
    : IRequestHandler<DeleteFriendRequest>
{
    public async Task Handle(DeleteFriendRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            throw new NotFoundException("User not found");
        
        var friend = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
        if (friend is null)
            throw new NotFoundException("Friend not found");
        
        var friendship = await friendshipRepository.GetFriendshipByIdsAsync(request.UserId, request.FriendId, cancellationToken);
        if (friendship is null)
            throw new NotFoundException("Friendship not found");
        
        var friendRequest = await friendRequestRepository.GetFriendRequestByIdsAsync(request.UserId, request.FriendId, cancellationToken);
        if (friendRequest is null || friendRequest.Status != FriendRequestStatus.Accepted)
            throw new NotFoundException("Friend request not found");
        
        friendRequest.Status = FriendRequestStatus.Pending;
        
        await friendshipRepository.DeleteAsync(friendship, cancellationToken);
        await friendRequestRepository.UpdateAsync(friendRequest, cancellationToken);
    }
}