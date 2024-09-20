using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Api.Controllers;

[Route("api/baskets")]
[ApiController]
public class BasketsController(IBasketRepository basketRepository, IMapper mapper) : ControllerBase
{
    [HttpGet("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
    {
        var basket = await basketRepository.GetBasketAsync(basketId);
        return basket is null ? new CustomerBasket(basketId) : Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto customerBasket)
    {
        var mappedBasket = mapper.Map<CustomerBasket>(customerBasket);
        var createdOrUpdatedBasket = await basketRepository.UpdateBasketAsync(mappedBasket);
        return createdOrUpdatedBasket is null ? BadRequest(new ApiResponse(400)) : Ok(createdOrUpdatedBasket);
    }


    [HttpDelete("{basketId}")]
    public async Task<ActionResult<bool>> DeleteBasket(string basketId)
    {
        return await basketRepository.DeleteBaskeyAsync(basketId);
    }

    [HttpPost("addItem")]
    public async Task<ActionResult<CustomerBasket>> AddItemToCustomerBasket(
        [FromQuery] string basketId,
        [FromBody] BasketItemDto product)
    {
        var mappedProduct = mapper.Map<BasketItem>(product);
        var customerBasket = await basketRepository.AddItemToBasketAsync(basketId, mappedProduct);
        return customerBasket is null ? BadRequest(new ApiResponse(400)) : Ok(customerBasket);
    }

    [HttpPut("{basketId}/updateItemQuantity")]
    public async Task<ActionResult<CustomerBasket>> UpdateItemQuantityInBasket(
        string basketId, int productId, int quantity)
    {
        var basket = await basketRepository.UpdateItemQuantityInBasketAsync(basketId, productId, quantity);
        return basket is null ? BadRequest(new ApiResponse(400)) : Ok(basket);
    }


    [HttpDelete("{basketId}/removeItem")]
    public async Task<ActionResult<CustomerBasket>> DeleteItemInBasket(string basketId, int productId)
    {
        var basket = await basketRepository.RemoveItemFromBasketAsync(basketId, productId);
        return basket is null ? BadRequest(new ApiResponse(400)) : Ok(basket);
    }
}