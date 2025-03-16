using AutoMapper;
using Identity.Application.Usecases.Users.Commands.Registration;
using Identity.Presentation.Models.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UsersController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request,
                                                               CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegistrationRequest>(request);

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<RegisterResponse>(response));
    }
}