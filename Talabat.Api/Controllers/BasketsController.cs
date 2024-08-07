﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Api.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BasketsController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketsController(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
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
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto customerBasket)
    {
        var mappedBasket = _mapper.Map<CustomerBasket>(customerBasket);
        var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
        return createdOrUpdatedBasket is null ? BadRequest(new ApiResponse(400)) : Ok(createdOrUpdatedBasket);
    }


    // Delete basket

    [HttpDelete("{basketId}")]
    public async Task<ActionResult<bool>> DeleteBasket(string basketId)
    {
        return await _basketRepository.DeleteBaskeyAsync(basketId);
    }


}
