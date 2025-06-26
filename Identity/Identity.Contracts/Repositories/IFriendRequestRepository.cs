using Identity.Domain;

namespace Identity.Contracts.Repositories;

public interface IFriendRequestRepository : IBaseRepository<FriendRequest>
{
    Task<bool> IsFriendRequestExistsAsync(string senderId, string receiverId, CancellationToken cancellationToken = default);
    
    Task<List<FriendRequest>> GetReceivedFriendRequestsAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<List<FriendRequest>> GetSentFriendRequestsAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<FriendRequest?> GetFriendRequestByIdsAsync(string senderId, string receiverId, CancellationToken cancellationToken = default);
}