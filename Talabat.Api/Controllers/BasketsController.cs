using Microsoft.AspNetCore.Mvc;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;

    public BasketsController(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    // Get or Recreate basket
    [HttpGet("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        return basket is null ? new CustomerBasket(basketId) : Ok(basket);
    }

    // Update or Create new basket

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasket customerBasket)
    {
        var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
        return createdOrUpdatedBasket is null ? BadRequest(new ApiResponse(400)) : Ok(createdOrUpdatedBasket);
    }


    // Delete basket

    [HttpDelete("{basketId}")]
    public async Task<ActionResult<bool>> DeleteBasket(string basketId)
    {
        return await _basketRepository.DeleteBaskeyAsync(basketId);
    }


}
