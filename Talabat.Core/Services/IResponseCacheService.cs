namespace Talabat.Core.Services;
public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan expireTime);
    Task<string?> GetCachedResponseAsync(string cacheKey);
}
