namespace Talabat.Core.Entities;

public class CustomerBasket
{
    public CustomerBasket(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; } = [];
}