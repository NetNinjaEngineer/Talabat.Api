using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product>
    {
        public ProductWithTypeAndBrandSpecifications(string? sort = null) : base()
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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

        }

        public ProductWithTypeAndBrandSpecifications(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }
    }
}
