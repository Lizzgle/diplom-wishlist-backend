using System.Security.Claims;
using AutoMapper;
using Event.Application.Usecases.Events.Commands.Create;
using Event.Application.Usecases.Events.Commands.Delete;
using Event.Application.Usecases.Events.Queries.GetAllForUser;
using Event.Application.Usecases.Invitations.Commands.AcceptInvitation;
using Event.Application.Usecases.Invitations.Commands.DeleteInvitation;
using Event.Application.Usecases.Invitations.Commands.RejectInvitation;
using Event.Application.Usecases.Invitations.Queries.GetByEventId;
using Event.Application.Usecases.Invitations.Queries.GetById;
using Event.Application.Usecases.Invitations.Queries.GetByUserId;
using Event.Presentation.Models.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event.Presentation.Controllers;

[Route("api/invitations")]
[ApiController]
public class InvitationController(IMapper mapper, IMediator mediator) : Controller
{
    [HttpPut("{id}/accept")]
    [Authorize]
    public async Task<IActionResult> AcceptInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var request = new AcceptInvitationRequest() { Id = id, UserId = userId! };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpPut("{id}/reject")]
    [Authorize]
    public async Task<IActionResult> RejectInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var request = new RejectInvitationRequest() { Id = id, UserId = userId! };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteInvitationRequest() { UserId = userId!, Id = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetEventByIdResponseModel>> GetInvitationById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetByIdRequest() { UserId = userId!, Id = id };
        
       var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetEventByIdResponseModel>(response));
    } 
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GetEventsResponseModel>> GetInvitationsByUserId(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetByUserIdRequest() { UserId = userId! };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetEventByIdResponseModel>(response));
    }
}