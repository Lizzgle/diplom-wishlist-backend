using Common.Exceptions;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ArgumentException = Common.Exceptions.ArgumentException;

namespace Identity.Application.Usecases.Account.Commands.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new NotFoundException("user not found");

        if (user.PasswordHash != request.Password)
            throw new ArgumentException("Passwords do not match");
            
        var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
        if (!result.Succeeded)
            throw new InvalidTokenException();
    }
}