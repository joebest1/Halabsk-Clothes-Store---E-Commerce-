namespace Halabsk.Net.Models;

public sealed class Product
{
    public int Id { get; init; }
    public string Category { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
}
