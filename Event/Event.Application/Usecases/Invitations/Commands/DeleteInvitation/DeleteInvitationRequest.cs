using MediatR;

namespace Event.Application.Usecases.Invitations.Commands.DeleteInvitation;

public class DeleteInvitationRequest : IRequest
{
    public required Guid Id { get; init; }

    public required string UserId { get; set; }
}