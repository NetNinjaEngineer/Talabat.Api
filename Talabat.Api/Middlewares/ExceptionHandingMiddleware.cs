using System.Net;
using System.Text.Json;
using Talabat.Api.Errors;

namespace Talabat.Api.Middlewares;

public class ExceptionHandingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
