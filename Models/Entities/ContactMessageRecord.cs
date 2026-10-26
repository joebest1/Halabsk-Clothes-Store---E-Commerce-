using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Models;

// The CLR namespace intentionally remains Halabsk.Net.Models so the existing
// EF Core migrations and model snapshot keep the same entity type names.
public sealed class ContactMessageRecord
{
    public string Id { get; init; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string ContactName { get; init; } = string.Empty;

    [Required]
    [StringLength(256)]
    [EmailAddress]
    public string ContactEmail { get; init; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string ContactMessage { get; init; } = string.Empty;

    public DateTime SentAtUtc { get; init; }

    // Optional: contact messages remain available to anonymous visitors.
    [StringLength(450)]
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }
}
