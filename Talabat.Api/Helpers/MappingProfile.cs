using AutoMapper;
using Talabat.Api.Helpers;
using Talabat.Core.Entities;

namespace Talabat.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(p => p.ProductBrand, options => options.MapFrom(p => p.ProductBrand.Name))
            .ForMember(p => p.ProductType, options => options.MapFrom(p => p.ProductType.Name))
            .ForMember(p => p.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>());


    }
}
