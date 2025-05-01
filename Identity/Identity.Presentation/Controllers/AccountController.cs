using System.Net;
using System.Security.Claims;
using AutoMapper;
using Common.Extensions;
using Common.Notification.Interfaces;
using Common.Notification.Models;
using Identity.Application.Usecases.Account.Commands.ConfirmEmail;
using Identity.Application.Usecases.Account.Commands.Login;
using Identity.Application.Usecases.Account.Commands.RefreshToken;
using Identity.Application.Usecases.Account.Commands.Registration;
using Identity.Presentation.Models;
using Identity.Presentation.Models.ForgotPassword;
using Identity.Presentation.Models.Login;
using Identity.Presentation.Models.RefreshToken;
using Identity.Presentation.Models.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ForgotPasswordRequest = Identity.Application.Usecases.Account.Commands.ForgotPassword.ForgotPasswordRequest;
using ResetPasswordRequest = Identity.Application.Usecases.Account.Commands.ResetPassword.ResetPasswordRequest;

namespace Identity.Presentation.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : Controller
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    private readonly IEmailServiceSender _emailServiceSender;

    public AccountController(IMapper mapper, IMediator mediator,
        IEmailServiceSender emailServiceSender)
    {
        _mapper = mapper;
        _mediator = mediator;
        _emailServiceSender = emailServiceSender;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseModel))]
    public async Task<ActionResult<RegisterResponseModel>> Register([FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegistrationRequest>(request);

        var response = await _mediator.Send(command, cancellationToken);
        
        var callbackUrl =  Url.Action(
            action: "ConfirmEmail",
            "Account", 
            new { email = request.Email, code = WebUtility.UrlEncode(response.Code) },
            protocol: HttpContext.Request.Scheme);

        await _emailServiceSender.SendEmailAsync(
            new SendEmailArgs()
            {
                Email = request.Email,
                Subject = "Confirm your email",
                Message = $"{callbackUrl}"
            }, cancellationToken);

        return Ok(new RegisterResponseModel() { Email = request.Email, Url = callbackUrl! });
    }
    
    [HttpGet("confirm-email")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfirmEmailRequest))]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string? email, [FromQuery] string? code, CancellationToken cancellationToken)
    {
        var decodedCode = WebUtility.UrlDecode(code);
        
        var command = new ConfirmEmailRequest() { Email = email, Code = decodedCode };

        await _mediator.Send(command, cancellationToken);
        
        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseModel))]
    public async Task<ActionResult<LoginResponseModel>> Login(
        [FromBody] LoginRequestModel request, CancellationToken cancellationToken)
    {
        var deviceType = Request.Headers.GetDeviceType();
        
        var command = _mapper.Map<LoginRequest>(request);
        command.DeviceType = deviceType;
        
        var response = await _mediator.Send(command, cancellationToken);
         
        return Ok(_mapper.Map<LoginResponse>(response)); 
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForgotPasswordResponseModel))]
    public async Task<ActionResult<ForgotPasswordResponseModel>> ForgotPassword([FromBody] string email,
        CancellationToken cancellationToken)
    {
        var command = new ForgotPasswordRequest() { Email = email };
        
        var response = await _mediator.Send(command, cancellationToken);
        
        var callbackUrl =  Url.Action(
            action: "ResetPassword",
            "Account", 
            new { Email = email, code = response.Code },
            protocol: HttpContext.Request.Scheme);

        await _emailServiceSender.SendEmailAsync(
            new SendEmailArgs()
            {
                Email = email,
                Subject = "Reset password",
                Message = $"{callbackUrl}"
            }, cancellationToken);
        
        return Ok(new ForgotPasswordResponseModel() { Email = email, Url = callbackUrl! });
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string code, 
        [FromBody] ResetPasswordRequestModel request, CancellationToken cancellationToken)
    {
        var command = new ResetPasswordRequest()
        {
            Email = email, 
            Code = code,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword
        };
        
        await _mediator.Send(command, cancellationToken);
        
        return Ok();
    }
    
    [HttpPost("refresh")]
    [Authorize]
    public async Task<ActionResult<RefreshTokenResponseModel>> Refresh([FromBody] string refreshToken, 
        CancellationToken cancellationToken)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new RefreshTokenRequest() { Id = id!, RefreshToken = refreshToken };
        
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<RefreshTokenResponseModel>(response));
    }
}