using Common.Models;
using Identity.Contracts.Models;

namespace Identity.Contracts.Services;

public interface IRedisService
{
    public Task SaveRefreshTokenAsync(SaveRefreshTokenArgs args, CancellationToken cancellationToken = default);
    
    public Task<string> GetRefreshTokenForDeviceAsync(string userId, DeviceType deviceType, CancellationToken cancellationToken = default);
}