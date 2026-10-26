using Halabsk.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halabsk.Net.Data.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.ToTable("CartItems", table =>
            table.HasCheckConstraint(
                "CK_CartItems_Quantity",
                $"[Quantity] >= 1 AND [Quantity] <= {CartRules.MaxQuantityPerItem}"));

        // A product can appear only once per cart.
        builder.HasIndex(item => new { item.CartId, item.ProductId })
            .IsUnique();

        builder.HasOne(item => item.Cart)
            .WithMany(cart => cart.Items)
            .HasForeignKey(item => item.CartId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Removing a product must not silently delete cart rows.
        builder.HasOne(item => item.Product)
            .WithMany()
            .HasForeignKey(item => item.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
