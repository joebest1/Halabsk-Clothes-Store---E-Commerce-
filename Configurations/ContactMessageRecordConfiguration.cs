using Halabsk.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halabsk.Net.Data.Configurations;

public sealed class ContactMessageRecordConfiguration : IEntityTypeConfiguration<ContactMessageRecord>
{
    public void Configure(EntityTypeBuilder<ContactMessageRecord> builder)
    {
        builder.HasKey(message => message.Id);

        builder.Property(message => message.Id)
            .HasMaxLength(64);

        builder.Property(message => message.ContactName)
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(message => message.ContactEmail)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(message => message.ContactMessage)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(message => message.SentAtUtc)
            .HasColumnType("datetime2");

        builder.Property(message => message.UserId)
            .HasMaxLength(450);

        // Deleting a user must not delete their contact history. Anonymous
        // submissions simply keep UserId null.
        builder.HasOne(message => message.User)
            .WithMany()
            .HasForeignKey(message => message.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(
            new ContactMessageRecord
            {
                Id = "a3a3e35f9b034f9184392bc6a0526f31",
                ContactName = "Codex QA",
                ContactEmail = "qa@example.com",
                ContactMessage = "Testing the migrated ASP.NET contact form.",
                SentAtUtc = new DateTime(2026, 5, 25, 9, 59, 59, 0000000, DateTimeKind.Utc)
            });
    }
}
