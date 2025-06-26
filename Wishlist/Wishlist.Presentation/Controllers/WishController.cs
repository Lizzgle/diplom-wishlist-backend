using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wishlist.Application.Usecases.Wishes.Commands.Create;
using Wishlist.Application.Usecases.Wishes.Commands.Delete;
using Wishlist.Application.Usecases.Wishes.Commands.Update;
using Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;
using Wishlist.Application.Usecases.Wishes.Queries.GetById;
using Wishlist.Presentation.Models.Wishes;

namespace Wishlist.Presentation.Controllers;

[Route("api/wishlists")]
[ApiController]
public class WishController(IMediator mediator, IMapper mapper) : Controller
{
    [HttpPost("{wishlistId}/wishes")]
    [Authorize]
    public async Task<IActionResult> Create([FromRoute] Guid wishlistId, [FromBody] CreateWishRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<CreateWishRequest>(request);
        command.CreatorId = userId!;
        command.WishlistId = wishlistId;
        
        await mediator.Send(command, cancellationToken);
        
        return Ok();
    } 
    
    [HttpPut("{wishlistId}/wishes/{id}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] Guid wishlistId, [FromRoute] Guid id, [FromBody] UpdateWishRequestModel request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = mapper.Map<UpdateWishRequest>(request);
        command.WishlistId = wishlistId;
        command.UserId = userId!;
        command.Id = id;
        
        await mediator.Send(command, cancellationToken);
        
        return Ok();
    } 
    
    [HttpDelete("{wishlistId}/wishes/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid wishlistId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new DeleteWishRequest() { UserId = userId!, WishlistId = wishlistId, WishId = id };
        
        await mediator.Send(request, cancellationToken);
        
        return Ok();
    } 
    
    [HttpGet("{wishlistId}/wishes/{id}")]
    [Authorize]
    public async Task<ActionResult<GetWishByIdResponseModel>> GetWishById([FromRoute] Guid wishlistId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetWishByIdRequest() { WishlistId = wishlistId, Id = id };
        
       var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetWishByIdResponseModel>(response));
    } 
    
    [HttpGet("{wishlistId}/wishes")]
    [Authorize]
    public async Task<ActionResult<GetBookedWishesByUserResponseModel>> GetBooledWishes(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var request = new GetBookedWishesByUserRequest() { UserId = userId! };
        
        var response = await mediator.Send(request, cancellationToken);
        
        return Ok(mapper.Map<GetBookedWishesByUserResponseModel>(response));
    }
}