using Event.Contracts.Models;
using Refit;

namespace Event.Contracts.HttpClients;

public interface IIdentityHttpClient
{
    [Get("/friends/{userId}")]
    Task<ApiResponse<GetUserFriendsResponse>> GetUserFriendsAsync(
        string userId,
        CancellationToken cancellationToken = default);
}