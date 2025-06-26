using AutoMapper;
using Core.Exceptions;
using Event.Contracts;
using MediatR;

namespace Event.Application.Usecases.Invitations.Queries.GetByEventId;

public class GetByEventIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetByEventIdRequest, GetByEventIdResponse>
{
    public async Task<GetByEventIdResponse> Handle(GetByEventIdRequest request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
            throw new NotFoundException("Event not found");
        
        if (@event.CreatorId != request.UserId)
            throw new ForbiddenException("You do not have access to this event");
        
        var invitations = await unitOfWork.InvitationRepository.GetInvitationsByEventIdAsync(request.EventId, cancellationToken);
        
        var invitationDtos = mapper.Map<List<InvitationByEventId>>(invitations);
        
        return new GetByEventIdResponse() { EventId = request.EventId, Invitations = invitationDtos };
    }
}