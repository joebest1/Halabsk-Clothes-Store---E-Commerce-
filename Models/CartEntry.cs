namespace Halabsk.Net.Models;

/// <summary>
/// Shared cart limits used by request validation and cart service logic.
/// </summary>
public static class CartRules
{
    public const int MaxQuantityPerItem = 99;
}

/// <summary>
/// Internal session cart storage. Only the product identifier and quantity are persisted;
/// product details and prices are always reloaded from the database.
/// </summary>
internal sealed class CartEntry
{
    public int ProductId { get; init; }
    public int Quantity { get; set; }
}
