using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specifications;
public class OrderSpecification : BaseSpecifications<Order>
{
    public OrderSpecification(string buyerEmail) : base(Order => Order.BuyerEmail == buyerEmail)
    {
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O => O.Items);
        AddOrderByDescending(O => O.OrderDate);
    }

    public OrderSpecification(string buyerEmail, int orderId) :
        base(Order => Order.BuyerEmail == buyerEmail && Order.Id == orderId)
    {
        Includes.Add(O => O.DeliveryMethod);
        Includes.Add(O => O.Items);
    }
}
