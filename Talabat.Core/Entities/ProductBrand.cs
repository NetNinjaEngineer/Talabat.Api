namespace Talabat.Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        public string? Name { get; set; }

        public ICollection<Product> Products { get; set; } = [];
    }
}
