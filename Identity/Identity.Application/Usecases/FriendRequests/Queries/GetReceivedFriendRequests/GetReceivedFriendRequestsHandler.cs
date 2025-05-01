using AutoMapper;
using Common.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;

public class GetReceivedFriendRequestsHandler(
    IFriendRequestRepository friendRequestRepository, UserManager<User> userManager, IMapper mapper) 
    : IRequestHandler<GetReceivedFriendRequest, GetReceivedFriendRequestsResponse>
{
    public async Task<GetReceivedFriendRequestsResponse> Handle(GetReceivedFriendRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new NotFoundException("User not found");
        
        var friendRequests = await friendRequestRepository.GetReceivedFriendRequestsAsync(request.UserId, cancellationToken);
        
        var friendRequestsDtos = mapper.Map<List<FriendRequestDto>>(friendRequests);
        return new GetReceivedFriendRequestsResponse() { FriendRequests = friendRequestsDtos };
    }
}