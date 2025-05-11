using AutoMapper;
using Event.Contracts;
using MediatR;

namespace Event.Application.Usecases.Events.Queries.GetAllForUser;

public class GetAllForUserHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<GetAllForUserRequest, GetAllForUserResponse>
{
    public async Task<GetAllForUserResponse> Handle(GetAllForUserRequest request, CancellationToken cancellationToken)
    {
        var eventIds = await unitOfWork.ParticipantRepository.GetEventIdsByUserIdAsync(request.UserId, cancellationToken);
        
        var events = await unitOfWork.EventRepository.GetEventsByIdsAsync(eventIds, cancellationToken);
        
        return mapper.Map<GetAllForUserResponse>(events);
    }
}