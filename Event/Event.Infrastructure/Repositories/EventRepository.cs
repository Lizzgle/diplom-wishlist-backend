using Event.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Event.Infrastructure.Repositories;

public class EventRepository(AppDbContext context) : BaseRepository<Domain.Event>(context), IEventRepository 
{
    public async Task<List<Domain.Event>> GetEventsByIdsAsync(IEnumerable<Guid> eventIds, CancellationToken cancellationToken)
    {
        return await context.Events
            .AsNoTracking() 
            .Where(e => eventIds.Contains(e.Id))
            .ToListAsync(cancellationToken);
    }
}