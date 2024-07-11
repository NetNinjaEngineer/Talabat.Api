using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Repository;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;
    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<bool> DeleteBaskeyAsync(string BasketId)
    {
        return await _database.KeyDeleteAsync(BasketId);
    }

    public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
    {
        var basket = await _database.StringGetAsync(BasketId);
        return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var jsonBasket = JsonSerializer.Serialize(basket);
        var CreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1));
        if (!CreatedOrUpdated) return null;
        return await GetBasketAsync(basket.Id!);
    }
}
