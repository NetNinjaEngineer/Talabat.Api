using AutoMapper;
using Talabat.Api.DTOs;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Api.Helpers;

public class OrderItemPictureUrlResolver(IConfiguration configuration)
    : IValueResolver<OrderItem, OrderItemDto, string>
{
    public string Resolve(
        OrderItem source,
        OrderItemDto destination,
        string destMember,
        ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ProductOrderItem.PictureUrl))
            return $"{configuration["ApiBaseUrl"]}/{source.ProductOrderItem.PictureUrl}";
        return string.Empty;
    }
}
