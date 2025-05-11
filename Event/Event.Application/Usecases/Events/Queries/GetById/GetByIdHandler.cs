using AutoMapper;
using Core.Exceptions;
using Event.Contracts;
using Event.Domain.Enums;
using MediatR;

namespace Event.Application.Usecases.Events.Queries.GetById;

public class GetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetByIdRequest, GetByIdResponse>
{
    public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
            throw new NotFoundException("Event not found");
        
        var participantIds = await unitOfWork.ParticipantRepository.GetUserIdsByEventIdAsync(request.EventId, cancellationToken);

        if (@event.CreatorId != request.UserId || !participantIds.Any(p => p == request.UserId))
            throw new ForbiddenException("You do not have access to this event.");
        
        var response = mapper.Map<GetByIdResponse>(@event);
        if (request.UserId == @event.CreatorId || @event.AccessLevel == AccessLevel.SelectedFriends)
            response.ParicipantIds = participantIds;
        else
            response.AccessLevel = null;
        
        return response;
    }
}