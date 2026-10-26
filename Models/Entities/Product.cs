using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Models;

// The CLR namespace intentionally remains Halabsk.Net.Models so the existing
// EF Core migrations and model snapshot keep the same entity type names.
public sealed class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    [Required]
    [StringLength(160)]
    public string Name { get; init; } = string.Empty;

    [Range(0.01, 999999.99)]
    public decimal Price { get; init; }

    [Required]
    [StringLength(500)]
    public string Description { get; init; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Image { get; init; } = string.Empty;
}
