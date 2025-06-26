using Core.Exceptions;
using Event.Contracts;
using MediatR;

namespace Event.Application.Usecases.Invitations.Commands.DeleteInvitation;

public class DeleteInvitationHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteInvitationRequest>
{

    public async Task Handle(DeleteInvitationRequest request, CancellationToken cancellationToken)
    {
        var invitation = await unitOfWork.InvitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException($"Invitation with id = { request.Id } not found");

        if (invitation.OrganizerId != request.UserId)
            throw new ForbiddenException("You do not have permission to delete this invitation");

        await unitOfWork.InvitationRepository.DeleteAsync(invitation);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}