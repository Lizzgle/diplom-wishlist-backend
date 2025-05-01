using Common.Exceptions;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.Account.Commands.ForgotPassword;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResponse>
{
    private readonly UserManager<User> _userManager;

    public ForgotPasswordHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            throw new NotFoundException("user not found");
        
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        return new ForgotPasswordResponse() { Code = code };
    }
}