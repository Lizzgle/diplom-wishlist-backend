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
}