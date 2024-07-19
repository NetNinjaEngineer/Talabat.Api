using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedDatabaseAsync(StoreContext storeContext)
        {
            if (!storeContext.ProductBrands.Any())
            {
                var brandsJsonData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var deSerializedBrandsData = JsonSerializer.Deserialize<List<ProductBrand>>(brandsJsonData);
                if (deSerializedBrandsData != null)
                    await storeContext.ProductBrands.AddRangeAsync(deSerializedBrandsData);
                await storeContext.SaveChangesAsync();
            }

            if (!storeContext.ProductTypes.Any())
            {
                var typesJsonData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var deSerializedTypesData = JsonSerializer.Deserialize<List<ProductType>>(typesJsonData);
                if (deSerializedTypesData != null)
                    await storeContext.ProductTypes.AddRangeAsync(deSerializedTypesData);
                await storeContext.SaveChangesAsync();
            }

            if (!storeContext.Products.Any())
            {
                var productsJsonData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var deSerializedProductsData = JsonSerializer.Deserialize<List<Product>>(productsJsonData);
                if (deSerializedProductsData != null)
                    await storeContext.Products.AddRangeAsync(deSerializedProductsData);
                await storeContext.SaveChangesAsync();
            }

            if (!storeContext.DeliveryMethods.Any())
            {
                var deliveryMethodsJson = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<IEnumerable<DeliveryMethod>>(deliveryMethodsJson);
                if (deliveryMethods is not null)
                    await storeContext.DeliveryMethods.AddRangeAsync(deliveryMethods);
                await storeContext.SaveChangesAsync();
            }
        }
    }
}
