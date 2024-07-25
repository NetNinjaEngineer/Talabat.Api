using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Services;

namespace Talabat.Service;
public class ResponseCacheService(IConnectionMultiplexer redis) : IResponseCacheService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan expireTime)
    {
        if (response is null) return;
        var Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var serializedResponse = JsonSerializer.Serialize(response, Options);
        await _database.StringSetAsync(cacheKey, serializedResponse, expireTime);
    }

    public async Task<string?> GetCachedResponseAsync(string cacheKey)
    {
        var CachedResponse = await _database.StringGetAsync(cacheKey);
        if (CachedResponse.IsNullOrEmpty) return null;
        return CachedResponse;
    }
}
