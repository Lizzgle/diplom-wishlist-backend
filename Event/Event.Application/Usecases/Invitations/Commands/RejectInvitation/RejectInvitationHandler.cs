using Core.Exceptions;
using Event.Contracts;
using Event.Domain.Enums;
using MediatR;

namespace Event.Application.Usecases.Invitations.Commands.RejectInvitation;

public class RejectInvitationHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RejectInvitationRequest>
{

    public async Task Handle(RejectInvitationRequest request, CancellationToken cancellationToken)
    {
        var invitation = await unitOfWork.InvitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation not found");

        if (invitation.InvitedId != request.UserId)
            throw new ForbiddenException("You do not have permission to accept this invitation");

        invitation.Status = InvitationStatus.Rejected;
        await unitOfWork.InvitationRepository.UpdateAsync(invitation, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
