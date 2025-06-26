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
using SecretSanta.Presentation.Models.Games;

namespace SecretSanta.Presentation.Controllers;

[Route("api/games")]
[ApiController]
public class GameController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateGameRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<CreateGameRequest>(request);
        command.CreatorId = userId!;
        
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
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteGameRequest() { UserId = userId!, GameId = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetGameByIdResponseModel>> GetEventById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetGameByIdRequest() { UserId = userId!, GameId = id };
        
       var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetGameByIdResponseModel>(response));
    } 
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GetAllGamesForUserResponseModel>> GetGames(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetAllGamesForUserRequest() { UserId = userId! };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetAllGamesForUserResponseModel>(response));
    }
    
    [HttpPatch("{id}/draw-lots")]
    [Authorize]
    public async Task<IActionResult> DrawLots([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DrawLotsRequest() { UserId = userId!, GameId = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    }
}