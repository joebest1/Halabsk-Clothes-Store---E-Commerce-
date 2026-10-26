namespace Halabsk.Net.ViewModels;

public sealed class ProductsPageViewModel
{
    public IReadOnlyList<ProductPreviewViewModel> PreviewProducts { get; init; } = [];
}
