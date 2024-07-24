using AutoMapper;
using Talabat.Api.DTOs;
using Talabat.Api.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.OrderAggregate;
using IdentityAddress = Talabat.Core.Entities.Identity.Address;
using OrderAddress = Talabat.Core.Entities.OrderAggregate.Address;

namespace Talabat.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(p => p.ProductBrand, options => options.MapFrom(p => p.ProductBrand.Name))
            .ForMember(p => p.ProductType, options => options.MapFrom(p => p.ProductType.Name))
            .ForMember(p => p.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>());

        CreateMap<AppUser, UserDto>();
        CreateMap<IdentityAddress, AddressDto>().ReverseMap();
        CreateMap<AddressDto, OrderAddress>();
        CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();

        CreateMap<Order, OrderToReturnDto>()
            .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
            .ForMember(dest => dest.DeliveryMethodCost, opt => opt.MapFrom(src => src.DeliveryMethod.Cost));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductOrderItem.ProductName))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductOrderItem.ProductId))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ProductOrderItem.PictureUrl))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());
    }
}
