using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Application.Usecases.Games.Commands.Create;
using SecretSanta.Application.Usecases.Games.Commands.Delete;
using SecretSanta.Application.Usecases.Games.Commands.DrawLots;
using SecretSanta.Application.Usecases.Games.Queries.GetAllForUser;
using SecretSanta.Application.Usecases.Games.Queries.GetById;
using SecretSanta.Application.Usecases.Players.Commands.Create;
using SecretSanta.Application.Usecases.Players.Queries.GetById;
using SecretSanta.Presentation.Models.Games;
using SecretSanta.Presentation.Models.Players;

namespace SecretSanta.Presentation.Controllers;


[Route("api/players")]
[ApiController]
public class PlayerController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePlayerRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<CreatePlayerRequest>(request);
        command.UserId = userId!;
        
        await mediator.Send(command, cancellationToken);
        
        return Ok();
    } 
    
    // [HttpPut("{id}")]
    // [Authorize]
    // public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEventRequestModel request, CancellationToken cancellationToken)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     
    //     var command = mapper.Map<UpdateEventRequest>(request);
    //     command.UserId = userId!;
    //     command.EventId = id;
    //     
    //     await mediator.Send(command, cancellationToken);
    //     
    //     return Ok();
    // } 
    
    // [HttpDelete("{id}")]
    // [Authorize]
    // public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     
    //     var request = new DeleteGameRequest() { UserId = userId!, GameId = id };
    //     
    //     await mediator.Send(request, cancellationToken);
    //     
    //     return Ok();
    // } 
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetPlayerByIdResponseModel>> GetPlayerById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetPlayerByIdRequest() { UserId = userId!, PlayerId = id };
        
       var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetPlayerByIdResponseModel>(response));
    } 
    
    // [HttpGet]
    // [Authorize]
    // public async Task<ActionResult<GetAllGamesForUserResponseModel>> GetEvents(CancellationToken cancellationToken)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     
    //     var request = new GetAllGamesForUserRequest() { UserId = userId! };
    //     
    //     var response = await mediator.Send(request, cancellationToken);
    //     
    //     return Ok(mapper.Map<GetAllGamesForUserResponseModel>(response));
    // }
    //
    // [HttpPatch("{id}/draw-lots")]
    // [Authorize]
    // public async Task<IActionResult> DrawLots([FromRoute] Guid id, CancellationToken cancellationToken)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     
    //     var request = new DrawLotsRequest() { UserId = userId!, GameId = id };
    //     
    //     await mediator.Send(request, cancellationToken);
    //     
    //     return Ok();
    // }
}