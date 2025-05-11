using Core.Exceptions;
using Event.Contracts;
using Event.Domain.Enums;
using MediatR;

namespace Event.Application.Usecases.Invitations.Commands.AcceptInvitation;

public class AcceptInvitationHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AcceptInvitationRequest>
{

    public async Task Handle(AcceptInvitationRequest request, CancellationToken cancellationToken)
    {
        var invitation = await unitOfWork.InvitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation not found");

        if (invitation.InvitedId != request.UserId)
            throw new ForbiddenException("You do not have permission to accept this invitation");

        invitation.Status = InvitationStatus.Accepted;
        await unitOfWork.InvitationRepository.UpdateAsync(invitation);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
