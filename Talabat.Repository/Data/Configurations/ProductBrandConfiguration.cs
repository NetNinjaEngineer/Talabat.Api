using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnType("varchar").HasMaxLength(100).IsRequired();
        }
    }
}
