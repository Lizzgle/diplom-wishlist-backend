using MediatR;

namespace Identity.Application.Usecases.Users.Commands.DeleteUser;

public class DeleteUserRequest : IRequest
{
    public required string Id { get; set; }
}