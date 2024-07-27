using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Status)
            .HasConversion(
                orderStatus => orderStatus.ToString(),
                orderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
                );

        builder.Property(p => p.SubTotal)
            .HasColumnType("decimal(18, 2)");

        builder.OwnsOne(order => order.ShippingAddress,
            options =>
            {
                options.Property(x => x.Street)
                    .HasColumnName("Street");

                options.Property(x => x.City)
                    .HasColumnName("City");

                options.Property(x => x.Country)
                    .HasColumnName("Country");

            });

        builder.HasOne(x => x.DeliveryMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


    }
}
