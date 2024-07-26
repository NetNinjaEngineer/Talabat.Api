using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.Api.Helpers;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DistributedCachedAttribute(int durationInSeconds) : Attribute, IAsyncActionFilter
{
    private readonly int durationInSeconds = durationInSeconds;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var distributedCacheService = context.HttpContext
            .RequestServices
            .GetRequiredService<IDistributedRedisCacheService>();

        var cacheKey = GenerateCacheKeyFromTheRequest(context.HttpContext.Request);
        var cachedResponse = await distributedCacheService.GetCachedResponseAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedResponse)) // response stored in the cache
        {
            var contentResponse = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200
            };

            context.Result = contentResponse;
            return;
        }

        var actionExecutedContext = await next.Invoke();
        if (actionExecutedContext.Result is OkObjectResult result)
            await distributedCacheService.CacheResponseAsync(cacheKey, result.Value, TimeSpan.FromSeconds(durationInSeconds), CancellationToken.None);

    }

    private static string GenerateCacheKeyFromTheRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append(request.Path);
        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            keyBuilder.Append($"|{key}-{value}");
        return keyBuilder.ToString();
    }
}
