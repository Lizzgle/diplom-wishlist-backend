using Identity.Domain;

namespace Identity.Contracts.Repositories;

public interface IFriendshipRepository : IBaseRepository<Friendship>
{
        
    Task<List<User>> GetFriendsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}