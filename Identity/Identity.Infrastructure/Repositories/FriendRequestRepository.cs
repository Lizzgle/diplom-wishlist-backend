using Identity.Contracts.Repositories;
using Identity.Domain;
using Identity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

// TODO add pagination

public class FriendRequestRepository(AppDbContext context) : 
    BaseRepository<FriendRequest>(context), IFriendRequestRepository
{
    
    public async Task<bool> IsFriendRequestExistsAsync(
        string senderId, string receiverId, 
        CancellationToken cancellationToken = default)
    {
        return await context.FriendRequests
                .AsNoTracking()
                .AnyAsync(f => f.SenderId == senderId && f.ReceiverId == receiverId, cancellationToken);
    }

    public async Task<List<FriendRequest>> GetReceivedFriendRequestsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.FriendRequests
            .AsNoTracking()
            .Where(f => f.ReceiverId == userId && f.Status == FriendRequestStatus.Pending)
            .Include(f => f.Sender)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<FriendRequest>> GetSentFriendRequestsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.FriendRequests
            .AsNoTracking()
            .Where(f => f.SenderId == userId && f.Status == FriendRequestStatus.Pending)
            .Include(f => f.Receiver)
            .ToListAsync(cancellationToken);
    }

    public async Task<FriendRequest?> GetFriendRequestByIdsAsync(string user1Id, string user2Id, CancellationToken cancellationToken = default)
    {
        return await context.FriendRequests
            .AsNoTracking()
            .Where(f => (f.SenderId == user1Id || f.SenderId == user2Id) && (f.ReceiverId == user1Id || f.ReceiverId == user2Id))
            .FirstOrDefaultAsync(cancellationToken);
    }
}