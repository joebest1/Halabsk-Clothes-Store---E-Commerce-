namespace Halabsk.Net.Responses;

public sealed class CartResponse
{
    public IReadOnlyList<CartItemResponse> Items { get; init; } = [];

    /// <summary>
    /// Total number of units across all cart items (sum of quantities), not the number of distinct products.
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// Sum of the server-computed line totals.
    /// </summary>
    public decimal Total { get; init; }
}
