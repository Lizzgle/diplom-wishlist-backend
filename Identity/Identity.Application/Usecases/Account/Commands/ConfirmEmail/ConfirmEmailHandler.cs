using System.Text;
using Core.Exceptions;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.Application.Usecases.Account.Commands.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailRequest>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ConfirmEmailHandler> _logger;

    public ConfirmEmailHandler(UserManager<User> userManager, ILogger<ConfirmEmailHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        if (request.Email is null || request.Code is null)
            throw new ArgumentException("Invalid email confirmation request.");

        _logger.LogInformation("Start handler ConfirmEmail {Email}", request.Email);
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
            throw new NotFoundException("User not found.");

        Console.WriteLine(request.Code);
        
        var result = await _userManager.ConfirmEmailAsync(user, request.Code.Replace(' ', '+'));
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning($"Email confirmation failed: {errors}");
            throw new InvalidTokenException();
        }
        
        _logger.LogInformation("Результат подтверждения: {Success}", result.Succeeded);
    }
}