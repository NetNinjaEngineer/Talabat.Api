using Talabat.Core.Entities;

namespace Talabat.Core.Specifications;

public class ProductWithFilterationCountSpec : BaseSpecifications<Product>
{
    public ProductWithFilterationCountSpec(ProductSpecParams Params)
        : base(p =>
            (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)
            &&
            (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
        )
    {

    }
}
