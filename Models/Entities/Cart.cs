using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Models;

// The CLR namespace intentionally remains Halabsk.Net.Models so the existing
// EF Core migrations and model snapshot keep the same entity type names.
public sealed class Cart
{
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public ICollection<CartItem> Items { get; } = new List<CartItem>();
}
