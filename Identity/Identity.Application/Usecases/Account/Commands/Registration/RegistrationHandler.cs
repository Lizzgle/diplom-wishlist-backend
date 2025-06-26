using AutoMapper;
using Core.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.Application.Usecases.Account.Commands.Registration;

public class RegistrationHandler(UserManager<User> userManager, IMapper mapper)
    : IRequestHandler<RegistrationRequest, RegistrationResponse>
{
    public async Task<RegistrationResponse> Handle(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var query = userManager.Users.Where(u => u.Email == request.Email || u.UserName == request.UserName);
        
        var users = await query.Where(u => u.EmailConfirmed).ToListAsync(cancellationToken);

        if (users.Count is not 0)
            throw new AlreadyExistException($"{request.Email} is already registered");

        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        var user = mapper.Map<User>(request);

        if (query.Count() is not 0)
            await userManager.UpdateAsync(user);
        else
        {
            await userManager.CreateAsync(user, request.Password);
            await userManager.AddToRoleAsync(user, "user");
        }
        
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        
        Console.WriteLine(code);

        return new RegistrationResponse() { Email = request.Email, Code = code };
    }
}