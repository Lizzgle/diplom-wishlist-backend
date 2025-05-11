using Event.Contracts.Repositories;

namespace Event.Contracts;

public interface IUnitOfWork
{
    IEventRepository EventRepository { get; }

    IInvitationRepository InvitationRepository { get; }
    
    IParticipantRepository ParticipantRepository { get; }
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}