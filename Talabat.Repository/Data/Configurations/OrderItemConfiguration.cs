using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18, 2)");

        builder.OwnsOne(x => x.ProductOrderItem, x => x.WithOwner());
    }
}
