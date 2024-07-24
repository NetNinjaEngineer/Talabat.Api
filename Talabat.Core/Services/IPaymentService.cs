using Talabat.Core.Entities;

namespace Talabat.Core.Services;
public interface IPaymentService
{
    Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
}
