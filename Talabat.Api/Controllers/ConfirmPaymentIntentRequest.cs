namespace Talabat.Api.Controllers;

public class ConfirmPaymentIntentRequest
{
    public string PaymentIntentId { get; set; }
    public string PaymentMethodId { get; set; }
}