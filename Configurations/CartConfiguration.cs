using Halabsk.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halabsk.Net.Data.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(cart => cart.Id);

        builder.Property(cart => cart.UserId)
            .HasMaxLength(450)
            .IsRequired();

        // One persistent cart per authenticated user.
        builder.HasIndex(cart => cart.UserId)
            .IsUnique();

        // A cart belongs to a user and is disposable if that user is removed.
        builder.HasOne(cart => cart.User)
            .WithOne()
            .HasForeignKey<Cart>(cart => cart.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
