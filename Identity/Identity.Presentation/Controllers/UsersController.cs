using System.Security.Claims;
using AutoMapper;
using Identity.Application.Usecases.Users.Commands.DeleteUser;
using Identity.Application.Usecases.Users.Queries.GetAllUsers;
using Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;
using Identity.Application.Usecases.Users.Queries.GetUserInfo;
using Identity.Application.Usecases.Users.Queries.GetUserName;
using Identity.Application.Usecases.Users.Queries.GetUsersNames;
using Identity.Presentation.Models;
using Identity.Presentation.Models.GetUserInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IMapper mapper, IMediator mediator) : Controller
{
    [HttpGet("filter")]
    public async Task<IActionResult> Get([FromQuery] string query, CancellationToken cancellationToken)
    {
        var request = new GetUsersByEmailOrNameRequest() { Query = query };
        
        var users = await mediator.Send(request, cancellationToken);
        
        return Ok(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetAllUsersRequest() { UserId = userId };
        
        var users = await mediator.Send(request, cancellationToken);
        
        return Ok(users);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteUserRequest() { Id = userId! };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    }

    [HttpGet("info")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetUserInfoResponseModel>> GetUserInfo(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetUserInfoRequest() { Id = userId! };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetUserInfoResponseModel>(response));
    }

    [HttpPost("usernames")]
    public async Task<ActionResult<List<GetUsersNamesResponseModel>>> GetUsersNames([FromBody] List<string> userIds,
        CancellationToken cancellationToken)
    {
        var request = new GetUsersNamesRequest() { UserIds = userIds };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<List<GetUsersNamesResponseModel>>(response));
    }
    
    [HttpGet("{userId}")]
    public async Task<ActionResult<string>> GetUserName([FromRoute] string userId,
        CancellationToken cancellationToken)
    {
        var request = new GetUserNameRequest() { UserId = userId };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(response);
    }
}