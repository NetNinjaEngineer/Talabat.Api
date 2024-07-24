using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services;
public interface IPaymentService
{
    Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
    Task<Order> UpdatePaymentIntentToSucceedOrFailed(string paymentIntentId, bool flag);
}
