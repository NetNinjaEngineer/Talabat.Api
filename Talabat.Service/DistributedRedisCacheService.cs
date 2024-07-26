using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Talabat.Core.Services;

namespace Talabat.Service;
public class DistributedRedisCacheService(IDistributedCache cache) : IDistributedRedisCacheService
{
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration, CancellationToken token)
    {
        if (response is null) return;
        var serializedResponse = JsonSerializer.Serialize(response);
        var CacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = duration
        };

        await cache.SetStringAsync(cacheKey, serializedResponse, CacheOptions, token);
    }

    public async Task<string?> GetCachedResponseAsync(string cacheKey)
    {
        var CachedResponse = await cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(CachedResponse))
            return CachedResponse;
        return null;
    }
}
