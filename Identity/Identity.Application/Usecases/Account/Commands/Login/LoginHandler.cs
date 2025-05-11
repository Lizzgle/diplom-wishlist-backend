using Core.Exceptions;
using Identity.Application.Providers;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.Account.Commands.Login;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginHandler(IJwtProvider jwtProvider, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _jwtProvider = jwtProvider;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        User? user;
        if (request.UniqueProperty.Contains('@'))
            user = await _userManager.FindByEmailAsync(request.UniqueProperty);
        else
            user = await _userManager.FindByNameAsync(request.UniqueProperty);

        if (user is null || !user.EmailConfirmed)
            throw new NotFoundException("User does not exist.");
        
        var result = await _signInManager.PasswordSignInAsync(
            user, request.Password, 
            isPersistent: false, lockoutOnFailure: false);
        
        if (!result.Succeeded)
            throw new InvalidAuthException("Username/email or password is incorrect.");

        var jwt = await _jwtProvider.GenerateJwtAsync(user);
        var refresh = await _jwtProvider.GetRefreshTokenForDeviceAsync(user.Id, request.DeviceType, cancellationToken) ??
                           await _jwtProvider.GenerateRefreshToken(user.Id, request.DeviceType, cancellationToken);
        
        return new LoginResponse() { Email = user.Email!, JwtToken = jwt, RefreshToken = refresh };
    }
}