using AutoMapper;
using Core.Exceptions;
using Event.Contracts;
using Event.Domain;
using MediatR;

namespace Event.Application.Usecases.Events.Commands.Update;

public class UpdateEventHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateEventRequest>
{
    public async Task Handle(UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
            throw new NotFoundException($"Event with id = { request.EventId } not found");

        if (@event.CreatorId != request.UserId)
            throw new ForbiddenException("You cannot update this event");
        
        var existParticipants = await unitOfWork.ParticipantRepository.GetAllByEventIdAsync(request.EventId, cancellationToken);
        var existingUserIds = existParticipants.Select(p => p.UserId).ToList();
        
        var userToAdd = request.UserIds
            .Where(p => !existingUserIds.Contains(p))
            .ToList();
        
        var participantToAdd = userToAdd.Select(userId => new Participant
        {
            UserId = userId,
            EventId = request.EventId,
        }).ToList();
        
        await unitOfWork.ParticipantRepository.AddRangeAsync(participantToAdd, cancellationToken);
        
        var userToDelete = existingUserIds
            .Where(existingUsers => !request.UserIds.Any(u => u == existingUsers) && existingUsers != @event.CreatorId)
            .ToList();
        
        var participantToDelete = userToDelete.Select(userId => new Participant
        {
            UserId = userId,
            EventId = request.EventId,
        }).ToList();
        
        await unitOfWork.ParticipantRepository.DeleteRangeAsync(participantToDelete, cancellationToken);
        
        await unitOfWork.EventRepository.UpdateAsync(mapper.Map<Domain.Event>(request), cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}