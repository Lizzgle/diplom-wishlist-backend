using Event.Contracts.Repositories;
using Event.Domain;
using Microsoft.EntityFrameworkCore;

namespace Event.Infrastructure.Repositories;

public class ParticipantRepository(AppDbContext context) : IParticipantRepository
{
    private readonly DbSet<Participant> _entities = context.Participants; 
    
    public async Task<List<Participant>> GetAllByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _entities.AsNoTracking().Where(p => p.EventId == eventId).ToListAsync(cancellationToken);
    }

    public async Task<List<string>> GetUserIdsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _entities
            .AsNoTracking()
            .Where(p => p.EventId == eventId)
            .Select(p => p.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Guid>> GetEventIdsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _entities
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.EventId)
            .ToListAsync(cancellationToken);
    }

    public Task AddAsync(Participant entity, CancellationToken cancellationToken = default)
    {
        _entities.Add(entity);
        return Task.CompletedTask;
    }
    
    public Task AddRangeAsync(List<Participant> participants, CancellationToken cancellationToken = default)
    {
        _entities.AddRange(participants);
        return Task.CompletedTask;
    }

    public  Task DeleteAsync(Participant entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(List<Participant> participants, CancellationToken cancellationToken = default)
    {
        _entities.RemoveRange(participants);
        return Task.CompletedTask;
    }
}