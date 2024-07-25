using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.Api.Helpers;

public class CachedAttribute(int expireTimeInSeconds) : Attribute, IAsyncActionFilter
{
    private readonly int expireTimeInSeconds = expireTimeInSeconds;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cachedResponse,
                StatusCode = 200,
                ContentType = "application/json"
            };

            context.Result = contentResult;
            return;
        }

        var actionExecutedContext = await next.Invoke();
        if (actionExecutedContext.Result is OkObjectResult result)
            await cacheService.CacheResponseAsync(cacheKey, result.Value, TimeSpan.FromSeconds(expireTimeInSeconds));


    }

    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append(request.Path); // api/products
        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            keyBuilder.Append($"|{key}-{value}"); // api/products|Sort-Name|PageIndex-1
        return keyBuilder.ToString();
    }
}
