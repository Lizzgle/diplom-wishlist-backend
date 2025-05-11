using Event.Domain;

namespace Event.Contracts.Repositories;

public interface IInvitationRepository : IBaseRepository<Invitation>
{
    Task<Invitation?> GetByIdWithIncludeAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<List<Invitation>> GetInvitationsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default);
    
    Task<List<Invitation>> GetInvitationsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}