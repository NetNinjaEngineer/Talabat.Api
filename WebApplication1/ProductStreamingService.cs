using System.Text.Json;

namespace WebApplication1;

public class ProductStreamingService
{
    private readonly HttpClient _httpClient;

    public ProductStreamingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async IAsyncEnumerable<string> StreamProducts()
    {
        var response = await _httpClient.GetAsync("https://localhost:7220/api/productstreaming/stream", HttpCompletionOption.ResponseHeadersRead);

        if (response.IsSuccessStatusCode)
        {
            await foreach (var product in JsonSerializer.DeserializeAsyncEnumerable<string>(await response.Content.ReadAsStreamAsync()))
            {
                yield return product;
            }
        }
    }
}