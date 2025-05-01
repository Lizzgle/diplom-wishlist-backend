using Common.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.FriendRequests.Commands.AcceptFriendRequest;

public class AcceptFriendRequestHandler(UserManager<User> userManager,
    IFriendRequestRepository friendRequestRepository, IFriendshipRepository friendshipRepository) 
    : IRequestHandler<AcceptFriendRequest>
{
    public async Task Handle(AcceptFriendRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.ReceiverId);
        if (user is null)
            throw new NotFoundException($"{request.ReceiverId} not found");
        
        var friendRequest = await friendRequestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (friendRequest is null)
            throw new NotFoundException("Friend request not found");

        if (friendRequest.ReceiverId != request.ReceiverId)
            throw new ForbiddenException("You cannot accept this request");

        friendRequest.Status = FriendRequestStatus.Accepted;
        friendRequest.RespondedAt = DateTime.UtcNow;
        await friendRequestRepository.UpdateAsync(friendRequest, cancellationToken);

        var friendship = new Friendship()
        {
            Friend1Id = friendRequest.ReceiverId,
            Friend2Id = friendRequest.SenderId,
            FriendsSince = DateTime.UtcNow
        };
        await friendshipRepository.AddAsync(friendship, cancellationToken);
    }
}