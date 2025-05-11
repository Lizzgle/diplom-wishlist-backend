using System.Security.Claims;
using AutoMapper;
using Event.Application.Usecases.Events.Commands.Create;
using Event.Application.Usecases.Events.Commands.Delete;
using Event.Application.Usecases.Events.Commands.Update;
using Event.Application.Usecases.Events.Queries.GetAllForUser;
using Event.Application.Usecases.Events.Queries.GetById;
using Event.Presentation.Models.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event.Presentation.Controllers;

[Route("api/events")]
[ApiController]
public class EventController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateEventRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<CreateEventRequest>(request);
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEventRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<UpdateEventRequest>(request);
        command.UserId = userId!;
        command.EventId = id;
        
        await mediator.Send(command, cancellationToken);
        
        return Ok();
    } 
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteEventRequest { UserId = userId!, EventId = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetEventByIdResponseModel>> GetEventById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetByIdRequest() { UserId = userId!, EventId = id };
        
       var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetEventByIdResponseModel>(response));
    } 
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GetEventsResponseModel>> GetEvents(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetAllForUserRequest() { UserId = userId! };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetEventByIdResponseModel>(response));
    }
}