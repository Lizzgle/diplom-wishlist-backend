using Core.Exceptions;
using Event.Contracts;
using MediatR;

namespace Event.Application.Usecases.Events.Commands.Delete;

public class DeleteEventHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventRequest>
{
    public async Task Handle(DeleteEventRequest request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
            throw new NotFoundException($"Event with id = { request.EventId } not found");

        if (@event.CreatorId != request.UserId)
            throw new ForbiddenException("You cannot delete this event");
        
        var participants = await unitOfWork.ParticipantRepository.GetAllByEventIdAsync(request.EventId, cancellationToken);
        
        await unitOfWork.ParticipantRepository.DeleteRangeAsync(participants, cancellationToken);
        await unitOfWork.EventRepository.DeleteAsync(@event, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}