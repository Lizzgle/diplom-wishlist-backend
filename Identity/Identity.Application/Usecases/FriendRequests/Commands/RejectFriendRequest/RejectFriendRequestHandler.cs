using Core.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Identity.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.FriendRequests.Commands.RejectFriendRequest;

public class RejectFriendRequestHandler(IFriendRequestRepository friendRequestRepository, UserManager<User> userManager) 
    : IRequestHandler<RejectFriendRequest>
{
    public async Task Handle(RejectFriendRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.ReceiverId);
        if (user is null)
            throw new NotFoundException($"{request.ReceiverId} not found");
        
        var friendRequest = await friendRequestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (friendRequest is null)
            throw new NotFoundException("Friend request not found");
        
        // TODO посмотреть возможность для отправителя щаявки отменить ее 
        if (friendRequest.ReceiverId != request.ReceiverId)
            throw new ForbiddenException("You cannot accept this request");

        friendRequest.Status = FriendRequestStatus.Rejected;
        friendRequest.RespondedAt = DateTime.UtcNow;
        await friendRequestRepository.UpdateAsync(friendRequest, cancellationToken);
    }
}