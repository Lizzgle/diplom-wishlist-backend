using Core.Exceptions;
using Identity.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Usecases.Users.Commands.DeleteUser;

public class DeleteUserHandler(UserManager<User> userManager) : IRequestHandler<DeleteUserRequest>
{
    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
            throw new NotFoundException($"User with id = {request.Id} not found");
        
        await userManager.DeleteAsync(user);
    }
}