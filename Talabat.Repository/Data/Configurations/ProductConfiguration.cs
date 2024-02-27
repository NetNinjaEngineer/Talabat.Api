using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).HasColumnType("varchar")
                .HasMaxLength(100).IsRequired();

            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.PictureUrl).IsRequired();

            builder.HasOne(p => p.ProductBrand).WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductBrandId);

            builder.Property(p => p.Price).HasColumnType("decimal(18, 2)").IsRequired();

            builder.HasOne(p => p.ProductType).WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductTypeId);

        }
    }
}
