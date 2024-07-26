namespace Talabat.Core.Services;
public interface IDistributedRedisCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration, CancellationToken token);
    Task<string?> GetCachedResponseAsync(string cacheKey);
}
