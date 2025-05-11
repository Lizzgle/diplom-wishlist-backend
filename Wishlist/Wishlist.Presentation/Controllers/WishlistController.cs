using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishlist.Application.Usecases.Wishlists.Commands.Create;
using Wishlist.Application.Usecases.Wishlists.Commands.Delete;
using Wishlist.Application.Usecases.Wishlists.Commands.Update;
using Wishlist.Application.Usecases.Wishlists.Queries.GetById;
using Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;
using Wishlist.Presentation.Models;
using Wishlist.Presentation.Models.Wishlists;

namespace Wishlist.Presentation.Controllers;

[Route("api/wishlist")]
[ApiController]
public class WishlistController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateWishlistRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<CreateWishlistRequest>(request);
        command.UserId = userId!;
        
        await mediator.Send(command, cancellationToken);
        
        return Ok();
    } 
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWishlistRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<UpdateWishlistRequest>(request);
        command.UserId = userId!;
        command.Id = id;
        
        await mediator.Send(command, cancellationToken);
        
        return Ok();
    } 
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteWishlistRequest() { UserId = userId!, Id = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetWishlistByIdResponseModel>> GetWishlistById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var request = new GetWishlistByIdRequest() { UserId = userId!, Id = id };
        
       var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetWishlistByIdResponseModel>(response));
    } 
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GetWishlistsByUserIdResponseModel>> GetWishlists(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetWishlistsByUserIdRequest() { UserId = userId! };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetWishlistsByUserIdResponseModel>(response));
    }
}