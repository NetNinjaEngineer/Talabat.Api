using AutoMapper;
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
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var MappedAddress = _mapper.Map<Address>(orderDto.ShippingAddress);
        var order = await _orderService.CreateOrderAsync(
            BuyerEmail,
            orderDto.BasketId,
            orderDto.DeliveryMethodId,
            MappedAddress);

        if (order is null) return BadRequest(new ApiResponse(400, "There is a problem with your order."));

        return Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
    {
        var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var Orders = await _orderService.GetOrderForSpecificUserAsync(BuyerEmail!);
        if (Orders is null) return NotFound(new ApiResponse(404, "There is no orders for this user."));
        return Ok(Orders);
    }

    [HttpGet("{orderId:int}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Order>> GetSpecificOrderForUser(int orderId)
    {
        var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
        var order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, orderId);
        if (order is null) return NotFound(new ApiResponse(404, $"There is no order with id: {orderId}"));
        return Ok(order);
    }

}
