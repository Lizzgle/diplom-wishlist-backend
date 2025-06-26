using Identity.Domain;

namespace Identity.Contracts.Repositories;

public interface IFriendshipRepository : IBaseRepository<Friendship>
{
        
    Task<List<User>> GetFriendsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<Friendship?> GetFriendshipByIdsAsync(string user1Id, string user2Id, CancellationToken cancellationToken = default);
}