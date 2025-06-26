using AutoMapper;
using Core.Exceptions;
using Event.Contracts;
using Event.Contracts.HttpClients;
using Event.Domain.Enums;
using MediatR;

namespace Event.Application.Usecases.Events.Queries.GetById;

public class GetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityHttpClient identityHttpClient) : IRequestHandler<GetByIdRequest, GetByIdResponse>
{
    public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.EventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event is null)
            throw new NotFoundException("Event not found");
        
        var participantIds = await unitOfWork.ParticipantRepository.GetUserIdsByEventIdAsync(request.EventId, cancellationToken);

        if (@event.CreatorId != request.UserId || !participantIds.Any(p => p == request.UserId))
            throw new ForbiddenException("You do not have access to this event.");

        var apiResponse = await identityHttpClient.GetUserName(@event.CreatorId, cancellationToken);
        if (!apiResponse.IsSuccessStatusCode || apiResponse.Content is null)
            throw new NotFoundException("User not found");
        
        var response = mapper.Map<GetByIdResponse>(@event);
        response.Creator = new Creator() { CreatorId = @event.CreatorId, Username = apiResponse.Content };

        if (request.UserId == @event.CreatorId || @event.AccessLevel == AccessLevel.SelectedFriends)
        {
            var identityResponse = await identityHttpClient.GetUsersNamesAsync(participantIds, cancellationToken);
            if (!identityResponse.IsSuccessStatusCode || identityResponse.Content is null)
                throw new NotFoundException("User not found");
            
            response.ParticipantDtos = identityResponse.Content.Select(identity => mapper.Map<ParticipantDto>(identity)).ToList();
        }
        else
            response.AccessLevel = null;
        
        return response;
    }
}