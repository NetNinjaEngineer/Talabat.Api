namespace Talabat.Core.Entities.OrderAggregate;
public class OrderItem : BaseEntity
{
    private OrderItem() { }
    public OrderItem(ProductOrderItem productOrderItem, decimal price, int quantity)
    {
        ProductOrderItem = productOrderItem;
        Price = price;
        Quantity = quantity;
    }

    public ProductOrderItem ProductOrderItem { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
