namespace Talabat.Core.Entities;

public class CustomerBasket(string id)
{
    public string Id { get; set; } = id;
    public string PaymentIntentId { get; set; }
    public string ClientSecret { get; set; }
    public int? DeliveryMethodId { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; } = [];
}