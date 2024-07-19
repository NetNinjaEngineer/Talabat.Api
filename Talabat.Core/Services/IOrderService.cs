using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services;
public interface IOrderService
{
    Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
    Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail);
    Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId);

}
