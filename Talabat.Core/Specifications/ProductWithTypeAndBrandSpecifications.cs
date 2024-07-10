using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product>
    {
        public ProductWithTypeAndBrandSpecifications(ProductSpecParams Params)
            : base(p =>
                (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue || p.ProductBrandId == Params.BrandId)
                &&
                (!Params.TypeId.HasValue || p.ProductTypeId == Params.TypeId)
            )
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            ApplyPagination(Params.PageSize, (Params.PageNumber - 1) * Params.PageSize);

        }

        public ProductWithTypeAndBrandSpecifications(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }
    }
}
