using Core.Exceptions;
using Identity.Application.Providers;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.Account.Commands.RefreshToken;

public class RefreshTokenHandler(UserManager<User> userManager, IJwtProvider jwtProvider) 
    : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user is null)
            throw new NotFoundException($"User by id = { request.Id } not found.");

        var refreshToken = await jwtProvider.GetRefreshTokenForDeviceAsync(user.Id, request.DeviceType, cancellationToken);
        if (refreshToken is null)
            throw new InvalidAuthException("Invalid refresh token.");

        var newJwt = await jwtProvider.GenerateJwtAsync(user, cancellationToken);

        return new RefreshTokenResponse() { Email = user.Email!, JwtToken = newJwt };
    }
}