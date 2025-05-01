using AutoMapper;
using Common.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.FriendRequests.Queries.GetSentFriendRequests;

public class GetSentFriendRequestsHandler(
    IFriendRequestRepository friendRequestRepository,
    UserManager<User> userManager,
    IMapper mapper)
    : IRequestHandler<GetSentFriendRequest, GetSentFriendRequestsResponse>
{

    public async Task<GetSentFriendRequestsResponse> Handle(GetSentFriendRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new NotFoundException("User not found");
        
        var friendRequests = await friendRequestRepository.GetSentFriendRequestsAsync(request.UserId, cancellationToken);
        
        var friendRequestsDtos = mapper.Map<List<FriendRequestDto>>(friendRequests);
        return new GetSentFriendRequestsResponse() { FriendRequests = friendRequestsDtos };
    }
}