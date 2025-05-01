using AutoMapper;
using Common.Exceptions;
using Identity.Contracts.Repositories;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArgumentException = Common.Exceptions.ArgumentException;

namespace Identity.Application.Usecases.Account.Commands.Registration;

public class RegistrationHandler : IRequestHandler<RegistrationRequest, RegistrationResponse>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public RegistrationHandler(UserManager<User> userManager, IMapper mapper, IUserRepository userRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<RegistrationResponse> Handle(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users.Where(u => u.Email == request.Email || u.UserName == request.UserName);
        
        var users = await query.Where(u => u.EmailConfirmed).ToListAsync(cancellationToken);

        if (users.Count is not 0)
            throw new AlreadyExistException($"{request.Email} is already registered");

        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        var user = _mapper.Map<User>(request);

        if (query.Count() is not 0)
            await _userManager.UpdateAsync(user);
        else
        {
            var t = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "user");
        }
        
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return new RegistrationResponse() { Email = request.Email, Code = code };
    }
}