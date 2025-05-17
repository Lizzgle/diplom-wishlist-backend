using Event.Contracts;
using Event.Contracts.Repositories;
using Event.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Event.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    private IEventRepository? _events;
    
    private IInvitationRepository? _invitations;
    
    private IParticipantRepository? _participants;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEventRepository EventRepository => _events ??= new EventRepository(_dbContext);

    public IInvitationRepository InvitationRepository => _invitations ??= new InvitationRepository(_dbContext);

    public IParticipantRepository ParticipantRepository => _participants ??= new ParticipantRepository(_dbContext);
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}