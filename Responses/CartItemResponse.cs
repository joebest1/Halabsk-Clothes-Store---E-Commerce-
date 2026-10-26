namespace Halabsk.Net.Responses;

public sealed class CartItemResponse
{
    /// <summary>
    /// The product identifier. The cart is keyed by product ID, not a separate cart-item ID.
    /// </summary>
    public int Id { get; init; }

    public string Category { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;

    /// <summary>
    /// Mutable because the cart service updates the hydrated response item when a quantity changes.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Computed from the trusted server-side product price and the validated quantity.
    /// </summary>
    public decimal LineTotal => Price * Quantity;
}
