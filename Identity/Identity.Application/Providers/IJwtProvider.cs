using Common.Models;
using Identity.Domain;

namespace Identity.Application.Providers;

public interface IJwtProvider
{
    public Task<string> GenerateJwtAsync(User user, CancellationToken cancellationToken = default);

    public Task<string> GenerateRefreshToken(string userId, DeviceType deviceType, CancellationToken cancellationToken = default);
    
    public Task<string> GetRefreshTokenForDeviceAsync(string userId, DeviceType deviceType, CancellationToken cancellationToken = default);
}