using Halabsk.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halabsk.Net.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.ProductName)
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(item => item.Quantity)
            .IsRequired();

        // LineTotal is calculated from the preserved unit price and quantity.
        builder.Ignore(item => item.LineTotal);

        builder.ToTable("OrderItems", table =>
            table.HasCheckConstraint("CK_OrderItems_Quantity", "[Quantity] > 0"));

        builder.HasOne(item => item.Order)
            .WithMany(order => order.Items)
            .HasForeignKey(item => item.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Keep historical order rows if a product is removed.
        builder.HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
