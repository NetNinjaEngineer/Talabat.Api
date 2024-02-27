using AutoMapper;
using Talabat.Core.Entities;

namespace Talabat.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(p => p.ProductBrand,
            options => options.MapFrom(p => p.ProductBrand.Name));
    }
}
