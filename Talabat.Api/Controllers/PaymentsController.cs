using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Api.DTOs;
using Talabat.Api.Errors;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController(
    IPaymentService paymentService,
    IMapper mapper,
    IConfiguration configuration) : ControllerBase
{
    const string endpointSecret = "whsec_ae81283d64b8015ba90590bcc738a6363f21fcac13030a6b384128e3a3cb134c";

    [HttpPost]
    [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var CustomerBasket = await paymentService.CreateOrUpdatePaymentIntent(basketId);
        if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "There is a problem with your basket"));
        var MappedCustomerBasket = mapper.Map<CustomerBasketDto>(CustomerBasket);
        return Ok(MappedCustomerBasket);
    }

    [HttpPost("confirm-payment-intent")]
    public ActionResult ConfirmPaymentIntent([FromBody] ConfirmPaymentIntentRequest request)
    {
        StripeConfiguration.ApiKey = configuration["StripeKeys:SecretKey"];

        var service = new PaymentIntentService();
        var paymentIntent = service.Confirm(
            request.PaymentIntentId,
            new PaymentIntentConfirmOptions
            {
                PaymentMethod = request.PaymentMethodId
            }
        );

        return Ok(paymentIntent);
    }

    [HttpPost("create-payment-method")]
    public ActionResult CreatePaymentMethod()
    {
        StripeConfiguration.ApiKey = configuration["StripeKeys:SecretKey"];
        var paymentMethod = GetPaymentMethod();

        return Ok(new { paymentMethodId = paymentMethod.Id });
    }



    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                await paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, false);
            }
            else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                await paymentService.UpdatePaymentIntentToSucceedOrFailed(paymentIntent.Id, true);
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }


    private static PaymentMethod GetPaymentMethod()
    {
        var options = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Token = "tok_visa"
            },
        };

        var service = new PaymentMethodService();
        PaymentMethod paymentMethod = service.Create(options);
        return paymentMethod;
    }

}
