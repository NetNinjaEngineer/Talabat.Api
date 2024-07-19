namespace Talabat.Core.Entities.OrderAggregate;
public class Order : BaseEntity
{
    private Order() { }
    public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
    {
        BuyerEmail = buyerEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        Items = items;
        SubTotal = subTotal;
    }

    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public Address ShippingAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
    public decimal SubTotal { get; set; } // price * quantity

    // delivery method cost  + subtotal
    public decimal GetTotal()
        => SubTotal * DeliveryMethod.Cost;
    public string PaymentIntentId { get; set; } = string.Empty;
}
