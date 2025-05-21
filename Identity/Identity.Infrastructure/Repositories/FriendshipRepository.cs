using Identity.Contracts.Repositories;
using Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class FriendshipRepository(AppDbContext context) 
    : BaseRepository<Friendship>(context), IFriendshipRepository
{
    
    public async Task<List<User>> GetFriendsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var query1 =  context.Friendships
            .AsNoTracking()
            .Where(f => f.Friend1Id == userId)
            .Select(f => f.Friend2);
        
        var query2 = context.Friendships
            .AsNoTracking()
            .Where(f => f.Friend2Id == userId)
            .Select(f => f.Friend1);
        
        return await query1.Union(query2).ToListAsync(cancellationToken);
    }

    public async Task<Friendship?> GetFriendshipByIdsAsync(string user1Id, string user2Id, CancellationToken cancellationToken = default)
    {
        return await context.Friendships
            .AsNoTracking()
            .Where(f => (f.Friend1Id == user1Id || f.Friend1Id == user2Id) && (f.Friend2Id == user1Id || f.Friend2Id == user2Id))
            .FirstOrDefaultAsync(cancellationToken);
    }
}