using Core.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.Application.Usecases.FriendRequests.Commands.SendFriendRequest;

public class SendFriendRequestHandler(IFriendRequestRepository friendRequestRepository, UserManager<User> userManager) 
    : IRequestHandler<SendFriendRequest>
{
    public async Task Handle(SendFriendRequest request, CancellationToken cancellationToken)
    {
        if (request.SenderId == request.ReceiverId)
            throw new ArgumentException("You cannot send a friend request to yourself");
        
        var isFriendRequestExists = await friendRequestRepository.IsFriendRequestExistsAsync(
            request.SenderId, request.ReceiverId, cancellationToken);
        if (isFriendRequestExists)
            throw new AlreadyExistException("Friend request already exists");
        
        var sender = await userManager.FindByIdAsync(request.SenderId);
        if (sender is null)
            throw new NotFoundException($"User with id = {request.SenderId} not found");
        
        var receiver = await userManager.FindByIdAsync(request.ReceiverId);
        if (receiver is null)
            throw new NotFoundException($"User with id = {request.ReceiverId} not found");

        var newFriendRequest = new FriendRequest()
        {
            Id = Guid.NewGuid(),
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId
        };
        await friendRequestRepository.AddAsync(newFriendRequest, cancellationToken);
    }
}