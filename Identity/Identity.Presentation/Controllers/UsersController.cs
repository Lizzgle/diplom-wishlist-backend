using System.Security.Claims;
using AutoMapper;
using Identity.Application.Usecases.Users.Commands.DeleteUser;
using Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IMapper mapper, IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string query, CancellationToken cancellationToken)
    {
        var request = new GetUsersByEmailOrNameRequest() { Query = query };
        
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
}