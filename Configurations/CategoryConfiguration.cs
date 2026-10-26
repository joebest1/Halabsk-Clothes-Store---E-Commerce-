using Halabsk.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halabsk.Net.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(category => category.Name)
            .IsUnique();

        // The three categories used by the existing storefront and admin form.
        // Explicit IDs keep the seeded products and migration history stable.
        builder.HasData(
            new Category { Id = 1, Name = "pants" },
            new Category { Id = 2, Name = "jackets" },
            new Category { Id = 3, Name = "tshirts" });
    }
}
