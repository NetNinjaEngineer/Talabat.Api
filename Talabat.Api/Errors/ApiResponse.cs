
namespace Talabat.Api.Errors;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public ApiResponse(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
    }

    private string? GetDefaultMessageForStatusCode(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        404 => "Resource Not Found",
        500 => "Internal Server Error",
        401 => "You are not authorized",
        _ => null
    };
}
