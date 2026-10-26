using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Models;

// The CLR namespace intentionally remains Halabsk.Net.Models so the existing
// EF Core migrations and model snapshot keep the same entity type names.
public sealed class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    // Nullable so deleting a product does not destroy historical orders.
    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    [StringLength(160)]
    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal => UnitPrice * Quantity;
}
