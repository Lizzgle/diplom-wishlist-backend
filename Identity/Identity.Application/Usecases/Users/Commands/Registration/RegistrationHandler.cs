using AutoMapper;
using Common.Exceptions;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArgumentException = Common.Exceptions.ArgumentException;

namespace Identity.Application.Usecases.Users.Commands.Registration;

public class RegistrationHandler : IRequestHandler<RegistrationRequest, RegistrationResponse>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public RegistrationHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<RegistrationResponse> Handle(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
                                      .Where(u => u.Email == request.Email || u.UserName == request.UserName)
                                      .ToListAsync();

        if (users.Count is not 0)
            throw new AlreadyExistException($"{request.Email} is already registered");

        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        var user = _mapper.Map<User>(request);

        await _userManager.CreateAsync(user, request.Password);

        return new RegistrationResponse() { Email = request.Email };
    }
}