﻿using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Service;
public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public OrderService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IPaymentService paymentService)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Order?> CreateOrderAsync(
        string buyerEmail,
        string basketId,
        int deliveryMethodId,
        Address shippingAddress)
    {
        // 1 Get basket from basket repo
        var basket = await _basketRepository.GetBasketAsync(basketId);
        // 2 Get selected items in basket from product repo
        var orderItems = new List<OrderItem>();
        if (basket?.BasketItems.Count > 0)
        {
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (product is not null)
                {
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
        }

        Order? order = null;

        if (orderItems.Count > 0)
        {
            // 3 calculate subtotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            // 4 get delivery method from delivery method repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            // 5 create order

            var exOrderSpec = new ExOrderSpecification(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecification(exOrderSpec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);
            // 6 add order locally
            await _unitOfWork.Repository<Order>().Add(order);
        }

        // 7 save order to database
        var result = await _unitOfWork.CompleteAsync();
        if (result <= 0)
            return null;

        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail)
    {
        var Spec = new OrderSpecification(buyerEmail);
        var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(Spec);
        return Orders;
    }

    public async Task<Order> GetOrderByIdForSpecificUserAsync(
        string buyerEmail, int orderId)
    {
        var Spec = new OrderSpecification(buyerEmail, orderId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecification(Spec);
        return order;
    }
}
