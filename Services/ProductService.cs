using Halabsk.Net.Data;
using Halabsk.Net.Models;
using Halabsk.Net.Requests;
using Halabsk.Net.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Halabsk.Net.Services;

public sealed class ProductService(AppDbContext dbContext, IMemoryCache cache)
{
    private const string AllProductsCacheKey = "products:all";
    private static readonly TimeSpan ProductCacheDuration = TimeSpan.FromMinutes(5);

    public async Task<IReadOnlyList<ProductPreviewViewModel>> GetAllAsync()
    {
        return (await GetCachedProductsAsync())
            .Select(ToPreview)
            .ToList();
    }

    public async Task<IReadOnlyList<ProductPreviewViewModel>> GetPreviewAsync(int count)
    {
        return (await GetCachedProductsAsync())
            .Select(ToPreview)
            .Take(count)
            .ToList();
    }

    public async Task<IReadOnlyList<ProductPreviewViewModel>> GetByCategoryAsync(string category)
    {
        var normalizedCategory = NormalizeCategory(category);

        return (await GetCachedProductsAsync())
            .Where(product => product.Category.Name == normalizedCategory)
            .Select(ToPreview)
            .ToList();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return (await GetCachedProductsAsync())
            .FirstOrDefault(product => product.Id == id);
    }

    public async Task<Product> CreateAsync(ProductCreateRequest request)
    {
        var categoryName = NormalizeCategory(request.Category);
        var category = await dbContext.Categories
            .FirstOrDefaultAsync(existing => existing.Name == categoryName);

        if (category is null)
        {
            category = new Category { Name = categoryName };
            dbContext.Categories.Add(category);
        }

        var product = new Product
        {
            Category = category,
            Name = request.Name.Trim(),
            Price = request.Price,
            Description = request.Description.Trim(),
            Image = request.Image.Trim()
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        cache.Remove(AllProductsCacheKey);

        return product;
    }

    private async Task<IReadOnlyList<Product>> GetCachedProductsAsync()
    {
        return await cache.GetOrCreateAsync(AllProductsCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = ProductCacheDuration;

            return await dbContext.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .OrderBy(product => product.Id)
                .ToListAsync();
        }) ?? [];
    }

    private static ProductPreviewViewModel ToPreview(Product product)
    {
        return new ProductPreviewViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Image = product.Image
        };
    }

    private static string NormalizeCategory(string category)
    {
        return category.Trim().ToLowerInvariant();
    }
}
