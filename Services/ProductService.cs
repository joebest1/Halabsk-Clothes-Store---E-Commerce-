using System.Text.Json;
using Halabsk.Net.Models;

namespace Halabsk.Net.Services;

public sealed class ProductService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
    private IReadOnlyList<Product>? _cache;

    public ProductService(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(environment.ContentRootPath, "Data", "products.json");
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        if (_cache is not null)
        {
            return _cache;
        }

        await _lock.WaitAsync();

        try
        {
            if (_cache is null)
            {
                await using var stream = File.OpenRead(_filePath);
                _cache = await JsonSerializer.DeserializeAsync<List<Product>>(stream, _jsonOptions) ?? [];
            }

            return _cache;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(string category)
    {
        return (await GetAllAsync())
            .Where(product => string.Equals(product.Category, category, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return (await GetAllAsync()).FirstOrDefault(product => product.Id == id);
    }
}
