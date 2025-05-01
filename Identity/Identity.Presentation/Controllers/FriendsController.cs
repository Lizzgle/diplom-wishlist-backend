using System.Security.Claims;
using AutoMapper;
using Identity.Application.Usecases.Friends.Queries.GetFriends;
using MediatR;
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
}