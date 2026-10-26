using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Models;

// The CLR namespace intentionally remains Halabsk.Net.Models so the existing
// EF Core migrations and model snapshot keep the same entity type names.
public sealed class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; } = new List<Product>();
}
