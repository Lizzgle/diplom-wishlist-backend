using MediatR;

namespace Event.Application.Usecases.Invitations.Commands.AcceptInvitation;

public class AcceptInvitationRequest : IRequest
{
    public required Guid Id { get; init; }

    public required string UserId { get; set; }
}