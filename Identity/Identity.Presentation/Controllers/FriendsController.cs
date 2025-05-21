using System.Security.Claims;
using AutoMapper;
using Identity.Application.Usecases.Friends.Commands.Delete;
using Identity.Application.Usecases.Friends.Queries.GetFriends;
using Identity.Application.Usecases.Users.Commands.DeleteUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/friends")]
[ApiController]
public class FriendsController (IMapper mapper, IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetMyFriends(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var request = new GetFriendsRequest() { UserId = userId! };

        var response = await mediator.Send(request, cancellationToken);

        return Ok(response);
    }   
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserFriends([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var request = new GetFriendsRequest() { UserId = userId! };

        var response = await mediator.Send(request, cancellationToken);

        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteFriendRequest() { UserId = userId!, FriendId = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    }
}