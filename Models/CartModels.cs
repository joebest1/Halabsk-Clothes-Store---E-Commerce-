namespace Halabsk.Net.Models;

public sealed class CartItem
{
    public int Id { get; init; }
    public string Category { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public int Quantity { get; set; }
    public decimal LineTotal => Price * Quantity;
}

public sealed class CartResponse
{
    public List<CartItem> Items { get; init; } = [];
    public int Count { get; init; }
    public decimal Total { get; init; }
}
