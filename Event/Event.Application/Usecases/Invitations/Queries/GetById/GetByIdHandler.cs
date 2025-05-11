using AutoMapper;
using Core.Exceptions;
using Event.Contracts;
using Event.Contracts.Repositories;
using MediatR;

namespace Event.Application.Usecases.Invitations.Queries.GetById;

public class GetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetByIdRequest, GetByIdResponse>
{

    public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var invitation = await unitOfWork.InvitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation not found");

        if (invitation.InvitedId != request.UserId && invitation.OrganizerId != request.UserId)
            throw new ForbiddenException("You do not have access to this invitation");

        return mapper.Map<GetByIdResponse>(invitation);
    }
}