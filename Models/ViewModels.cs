namespace Halabsk.Net.Models;

public sealed class CategoryPageViewModel
{
    public string Title { get; init; } = string.Empty;
    public string Tag { get; init; } = string.Empty;
    public string Heading { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string SearchPlaceholder { get; init; } = string.Empty;
}

public sealed class ProductsPageViewModel
{
    public IReadOnlyList<Product> PreviewProducts { get; init; } = [];
}

public sealed class AddToCartRequest
{
    public int ProductId { get; init; }
}

public sealed class UpdateCartQuantityRequest
{
    public int Quantity { get; init; }
}
