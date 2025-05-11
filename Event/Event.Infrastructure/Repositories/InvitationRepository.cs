using Event.Contracts.Repositories;
using Event.Domain;
using Microsoft.EntityFrameworkCore;

namespace Event.Infrastructure.Repositories;

public class InvitationRepository(AppDbContext context) : BaseRepository<Invitation>(context), IInvitationRepository
{
    public async Task<Invitation?> GetByIdWithIncludeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Invitations.AsNoTracking()
            .Where(i => i.Id == id)
            .Include(x => x.Event)
            .Include(x => x.Organizer)
            .Include(x => x.Invited)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Invitation>> GetInvitationsByEventIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await context.Invitations.AsNoTracking()
            .Where(i => i.EventId == eventId)
            .Include(x => x.Invited)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Invitation>> GetInvitationsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Invitations.AsNoTracking()
            .Where(i => i.OrganizerId == userId)
            .Include(x => x.Invited)
            .Include(i => i.Event)
            .ToListAsync(cancellationToken);
    }
}