using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;
public class HomeController : Controller
{
    private readonly ProductStreamingService _productStreamingService;

    public HomeController(ProductStreamingService productStreamingService)
    {
        _productStreamingService = productStreamingService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Home/StreamProducts")]
    public async IAsyncEnumerable<string> StreamProducts()
    {
        await foreach (var product in _productStreamingService.StreamProducts())
        {
            yield return product;
        }
    }
}
