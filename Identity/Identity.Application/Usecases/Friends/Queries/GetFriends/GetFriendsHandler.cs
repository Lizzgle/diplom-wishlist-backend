using AutoMapper;
using Common.Exceptions;
using Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;
using Identity.Contracts.Repositories;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.Friends.Queries.GetFriends;

public class GetFriendsHandler(UserManager<User> userManager, IFriendshipRepository friendshipRepository, IMapper mapper) 
    : IRequestHandler<GetFriendsRequest, GetFriendsResponse>
{
    public async Task<GetFriendsResponse> Handle(GetFriendsRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new NotFoundException("User not found");
        
        var friendships = await friendshipRepository.GetFriendsByUserIdAsync(request.UserId, cancellationToken);
        
        var friends = mapper.Map<List<FriendDto>>(friendships);
        return new GetFriendsResponse() { Friends = friends };
    }
}