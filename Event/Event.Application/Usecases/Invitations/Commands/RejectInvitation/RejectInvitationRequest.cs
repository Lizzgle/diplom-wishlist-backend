using MediatR;

namespace Event.Application.Usecases.Invitations.Commands.RejectInvitation;

public class RejectInvitationRequest : IRequest
{
    public required Guid Id { get; init; }

    public required string UserId { get; set; }
}