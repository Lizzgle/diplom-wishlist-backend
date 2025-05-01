using System.Security.Claims;
using AutoMapper;
using Identity.Application.Usecases.FriendRequests.Commands.AcceptFriendRequest;
using Identity.Application.Usecases.FriendRequests.Commands.RejectFriendRequest;
using Identity.Application.Usecases.FriendRequests.Commands.SendFriendRequest;
using Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;
using Identity.Application.Usecases.FriendRequests.Queries.GetSentFriendRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/friend-requests")]
[ApiController]
public class FriendRequestsController(IMapper mapper, IMediator mediator) : Controller
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendFriendRequest([FromBody] string receiverId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new SendFriendRequest() { SenderId = userId!, ReceiverId = receiverId };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    }
    
    [HttpPatch("{id:guid}/accept")]
    [Authorize]
    public async Task<IActionResult> AcceptFriendRequest([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new AcceptFriendRequest() { Id = id, ReceiverId = userId! };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    }
    
    [HttpPatch("{id:guid}/reject")]
    [Authorize]
    public async Task<IActionResult> RejectFriendRequest([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new RejectFriendRequest() { Id = id, ReceiverId = userId! };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    }

    [HttpGet("sent")]
    public async Task<IActionResult> GetSentFriendRequests(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var request = new GetSentFriendRequest() { UserId = userId! };

        var response = await mediator.Send(request, cancellationToken);

        return Ok(response);
    }
    
    [HttpGet("received")]
    public async Task<IActionResult> GetReceivedFriendRequests(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var request = new GetReceivedFriendRequest() { UserId = userId! };

        var response = await mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}