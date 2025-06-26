using AutoMapper;
using Core.Exceptions;
using Event.Contracts;
using Event.Contracts.HttpClients;
using Event.Domain;
using Event.Domain.Enums;
using MediatR;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Event.Application.Usecases.Events.Commands.Create;

public class CreateEventHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityHttpClient identityHttpClient) 
    : IRequestHandler<CreateEventRequest>
{
    public async Task Handle(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var @event = mapper.Map<Domain.Event>(request);
        
        await unitOfWork.EventRepository.AddAsync(@event, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var participant = new Participant() { EventId = @event.Id, UserId = request.CreatorId };
        
        await unitOfWork.ParticipantRepository.AddAsync(participant, cancellationToken);

        if (request.AccessLevel is AccessLevel.Friends)
        {
            var response = await identityHttpClient.GetUserFriendsAsync(request.CreatorId, cancellationToken);
            if (!response.IsSuccessful || response.Content is null)
                throw new NotFoundException("User friends not found");

            var friends = response.Content.UserFriends;
            var participants = friends.Select(p => new Participant
            {
                UserId = p.UserId,
                EventId = @event.Id
            }).ToList();
            
            if (participants.Count is not 0)
                await unitOfWork.ParticipantRepository.AddRangeAsync(participants, cancellationToken);
        }

        if (request.AccessLevel is AccessLevel.SelectedFriends)
        {
            if (request.UserIds.Count is 0)
                throw new ArgumentException($"With access level {request.AccessLevel} requires user ids");
            
            var participants = request.UserIds.Select(p => new Participant
            {
                UserId = p,
                EventId = @event.Id
            }).ToList();
            
            await unitOfWork.ParticipantRepository.AddRangeAsync(participants, cancellationToken);
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    
    //TODO содержать эту таблицу в актуальносм состоянии
}