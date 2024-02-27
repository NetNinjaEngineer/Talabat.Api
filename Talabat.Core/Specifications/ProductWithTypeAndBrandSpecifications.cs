using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product>
    {
        public ProductWithTypeAndBrandSpecifications() : base()
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }

        public ProductWithTypeAndBrandSpecifications(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }
    }
}
