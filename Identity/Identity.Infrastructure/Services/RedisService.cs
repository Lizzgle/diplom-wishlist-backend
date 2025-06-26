using Core.Models;
using Identity.Contracts.Models;
using Identity.Contracts.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Identity.Infrastructure.Services;

public class RedisService : IRedisService
{
    private readonly IDistributedCache _distributedCache;

    public RedisService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    
    public async Task SaveRefreshTokenAsync(SaveRefreshTokenArgs args, CancellationToken cancellationToken = default)
    {
        string redisKey = $"refresh_token:{args.UserId}:{args.DeviceType}";

        await _distributedCache.SetStringAsync(
            redisKey, 
            args.Token, 
            new DistributedCacheEntryOptions 
            { 
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) 
            }, cancellationToken);
    }

    public async Task<string> GetRefreshTokenForDeviceAsync(string userId, DeviceType deviceType,
        CancellationToken cancellationToken = default)
    {
        string redisKey = $"refresh_token:{userId}:{deviceType}";

        return await _distributedCache.GetStringAsync(redisKey, cancellationToken);
    }
}