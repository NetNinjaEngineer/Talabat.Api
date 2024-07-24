using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.DTOs;

public class CustomerBasketDto
{

    [Required]
    public string Id { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public int? DeliveryMethodId { get; set; }
    public ICollection<BasketItemDto> BasketItems { get; set; } = [];

}
