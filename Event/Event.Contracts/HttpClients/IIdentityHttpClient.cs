using Event.Contracts.Models;
using Refit;

namespace Event.Contracts.HttpClients;

public interface IIdentityHttpClient
{
    [Get("/api/friends/{userId}")]
    Task<ApiResponse<GetUserFriendsResponse>> GetUserFriendsAsync(
        string userId,
        CancellationToken cancellationToken = default);
    
    [Post("/api/users/usernames")]
    Task<ApiResponse<List<GetUsersNamesResponse>>> GetUsersNamesAsync(
        List<string> userIds,
        CancellationToken cancellationToken = default);
    
    [Get("/api/users/{userId}")]
    Task<ApiResponse<string>> GetUserName(
        string userId,
        CancellationToken cancellationToken = default);
}