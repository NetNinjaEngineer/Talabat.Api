﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers;

[Authorize]
public class OrdersController : APIBaseController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
    {
        var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var MappedAddress = _mapper.Map<Address>(orderDto.ShippingAddress);
        var order = await _orderService.CreateOrderAsync(
            BuyerEmail,
            orderDto.BasketId,
            orderDto.DeliveryMethodId,
            MappedAddress);

        if (order is null) return BadRequest(new ApiResponse(400, "There is a problem with your order."));
        var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
        return Ok(mappedOrder);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
    {
        var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var Orders = await _orderService.GetOrderForSpecificUserAsync(BuyerEmail!);
        if (Orders is null) return NotFound(new ApiResponse(404, "There is no orders for this user."));
        var MappedOrders = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(Orders);
        return Ok(MappedOrders);
    }

    [HttpGet("{orderId:int}")]
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderToReturnDto>> GetSpecificOrderForUser(int orderId)
    {
        var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, orderId);
        if (order is null) return NotFound(new ApiResponse(404, $"There is no order with id: {orderId}"));
        var mappedOrder = _mapper.Map<OrderToReturnDto>(order);
        return Ok(mappedOrder);
    }

}
