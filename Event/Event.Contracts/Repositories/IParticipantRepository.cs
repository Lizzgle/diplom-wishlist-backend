using Event.Domain;

namespace Event.Contracts.Repositories;

public interface IParticipantRepository
{
    Task<List<Participant>> GetAllByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    
    Task<List<string>> GetUserIdsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    
    Task<List<Guid>> GetEventIdsByUserIdAsync(string userId, CancellationToken cancellationToken = default);

    Task AddAsync(Participant entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(List<Participant> participants, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Participant entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(List<Participant> participants, CancellationToken cancellationToken = default);
}