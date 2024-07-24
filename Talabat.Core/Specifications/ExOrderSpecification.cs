using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications;
public class ExOrderSpecification : BaseSpecifications<Order>
{
    public ExOrderSpecification(string paymentIntentId) : base(Order => Order.PaymentIntentId == paymentIntentId)
    {

    }
}
