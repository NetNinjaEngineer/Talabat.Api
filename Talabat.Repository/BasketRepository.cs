using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Repository;

public class BasketRepository(IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteBaskeyAsync(string basketId)
    {
        return await _database.KeyDeleteAsync(basketId);
    }

    public async Task<CustomerBasket?> AddItemToBasketAsync(string basketId, BasketItem item)
    {
        var basket = await GetBasketAsync(basketId) ?? new CustomerBasket(basketId);

        var existingItem = basket.BasketItems.FirstOrDefault(x => x.Id == item.Id);

        if (existingItem != null)
            existingItem.Quantity += item.Quantity;
        else
            basket.BasketItems.Add(item);

        return await UpdateBasketAsync(basket);
    }

    public async Task<CustomerBasket?> RemoveItemFromBasketAsync(string basketId, int itemId)
    {
        var basket = await GetBasketAsync(basketId);

        if (basket == null) return null;

        var basketItem = basket.BasketItems.FirstOrDefault(x => x.Id == itemId);

        if (basketItem != null)
            basket.BasketItems.Remove(basketItem);

        return await UpdateBasketAsync(basket);
    }

    public async Task<CustomerBasket?> UpdateItemQuantityInBasketAsync(string basketId, int itemId, int quantity)
    {
        var basket = await GetBasketAsync(basketId);
        if (basket == null) return null;

        var itemToUpdate = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);
        if (itemToUpdate == null) return await UpdateBasketAsync(basket);
        if (quantity <= 0)
            basket.BasketItems.Remove(itemToUpdate);
        else
            itemToUpdate.Quantity = quantity;
        return await UpdateBasketAsync(basket);
    }

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var basket = await _database.StringGetAsync(basketId);
        return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var jsonBasket = JsonSerializer.Serialize(basket);
        var createdOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1));
        if (!createdOrUpdated) return null;
        return await GetBasketAsync(basket.Id!);
    }
}