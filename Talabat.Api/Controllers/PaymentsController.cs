using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController(
    IPaymentService paymentService,
    IMapper mapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var CustomerBasket = await paymentService.CreateOrUpdatePaymentIntent(basketId);
        if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "There is a problem with your basket"));
        var MappedCustomerBasket = mapper.Map<CustomerBasketDto>(CustomerBasket);
        return Ok(MappedCustomerBasket);
    }


}
