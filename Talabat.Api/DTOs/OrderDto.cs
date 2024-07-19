namespace Talabat.Api.DTOs;

public class OrderDto
{
    public string BasketId { get; set; }
    public AddressDto ShippingAddress { get; set; }
    public int DeliveryMethodId { get; set; }
}
