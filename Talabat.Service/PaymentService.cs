using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service;
public class PaymentService(
    IConfiguration configuration,
    IBasketRepository basketRepository,
    IUnitOfWork unitOfWork) : IPaymentService
{
    public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = configuration["StripeKeys:SecretKey"];
        var basket = await basketRepository.GetBasketAsync(basketId);
        if (basket is null) return null;

        decimal shippingPrice = 0M;

        if (basket.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
            shippingPrice = deliveryMethod.Cost;

        }

        if (basket.BasketItems.Count > 0)
        {
            foreach (var item in basket.BasketItems)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }
        }

        var subTotal = basket.BasketItems.Sum(item => item.Quantity * item.Price);

        // Create payment intent

        var service = new PaymentIntentService();
        PaymentIntent paymentIntent;
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var Options = new PaymentIntentCreateOptions
            {
                Amount = (long)(subTotal * 100 + shippingPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };

            paymentIntent = await service.CreateAsync(Options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            var Options = new PaymentIntentUpdateOptions
            {
                Amount = (long)(subTotal * 100 + shippingPrice * 100)
            };

            paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, Options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        await basketRepository.UpdateBasketAsync(basket);
        return basket;

    }
}
