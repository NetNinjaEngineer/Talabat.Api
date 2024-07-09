using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;

namespace Talabat.Api.Controllers;
[Route("errors/{code}")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    public ActionResult Error(int code)
    {
        return NotFound(new ApiResponse(code));
    }
}
