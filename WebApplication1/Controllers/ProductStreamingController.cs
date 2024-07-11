using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplication1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductStreamingController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ProductStreamingController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("stream")]
    public async Task StreamProducts()
    {
        Response.ContentType = "application/json";

        var response = await _httpClient.GetAsync("https://api.talabat.com/products"); // Replace with the actual Talabat API URL
        var stream = await response.Content.ReadAsStreamAsync();

        await foreach (var product in JsonSerializer.DeserializeAsyncEnumerable<Dictionary<string, object>>(stream))
        {
            var productJson = JsonSerializer.Serialize(product);
            await Response.WriteAsync($"data: {productJson}\n\n");
            await Response.Body.FlushAsync();
        }
    }
}