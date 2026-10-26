using Halabsk.Net.Data;
using Halabsk.Net.Extensions;
using Halabsk.Net.Models;
using Halabsk.Net.Responses;
using Microsoft.EntityFrameworkCore;

namespace Halabsk.Net.Services;

public sealed class CartService
{
    private const string CartKey = "Cart";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ProductService _productService;
    private readonly AppDbContext _dbContext;

    public CartService(
        IHttpContextAccessor httpContextAccessor,
        ProductService productService,
        AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
        _dbContext = dbContext;
    }

    public async Task<CartResponse> GetResponseAsync()
    {
        var userId = GetCurrentUserId();

        return userId is null
            ? await GetSessionResponseAsync()
            : await GetPersistentResponseAsync(userId);
    }

    public async Task<(bool Success, string Message, CartResponse Response)> AddAsync(int productId)
    {
        var userId = GetCurrentUserId();

        return userId is null
            ? await AddToSessionAsync(productId)
            : await AddToPersistentCartAsync(userId, productId);
    }

    public async Task<CartResponse> UpdateQuantityAsync(int productId, int quantity)
    {
        var userId = GetCurrentUserId();

        return userId is null
            ? await UpdateSessionQuantityAsync(productId, quantity)
            : await UpdatePersistentQuantityAsync(userId, productId, quantity);
    }

    public async Task<CartResponse> RemoveAsync(int productId)
    {
        var userId = GetCurrentUserId();

        return userId is null
            ? await RemoveFromSessionAsync(productId)
            : await RemoveFromPersistentCartAsync(userId, productId);
    }

    public async Task<CartResponse> ClearAsync()
    {
        var userId = GetCurrentUserId();

        if (userId is not null)
        {
            var cart = await _dbContext.Carts
                .Include(existing => existing.Items)
                .FirstOrDefaultAsync(existing => existing.UserId == userId);

            if (cart is not null)
            {
                _dbContext.Carts.Remove(cart);
                await _dbContext.SaveChangesAsync();
            }
        }

        Session.Remove(CartKey);

        return ToResponse([]);
    }

    /// <summary>
    /// Merges the anonymous session cart into the authenticated user's persistent
    /// cart. Called immediately after a successful login, while the HTTP session
    /// is still available. Invalid products and invalid quantities are dropped.
    /// </summary>
    public async Task MergeSessionCartAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return;
        }

        var (storedEntries, _) = ReadStoredEntries();

        if (storedEntries.Count == 0)
        {
            Session.Remove(CartKey);
            return;
        }

        var cart = await LoadPersistentCartForUpdateAsync(userId);
        var isNewCart = cart is null;
        cart ??= new Cart { UserId = userId };

        var changed = false;

        foreach (var storedEntry in storedEntries)
        {
            if (storedEntry.ProductId <= 0 || storedEntry.Quantity <= 0)
            {
                continue;
            }

            var quantity = Math.Min(storedEntry.Quantity, CartRules.MaxQuantityPerItem);
            var product = await _productService.GetByIdAsync(storedEntry.ProductId);

            if (product is null)
            {
                continue;
            }

            var item = cart.Items.FirstOrDefault(existing => existing.ProductId == storedEntry.ProductId);

            if (item is null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity
                });

                changed = true;
                continue;
            }

            var mergedQuantity = Math.Min(item.Quantity + quantity, CartRules.MaxQuantityPerItem);

            if (mergedQuantity != item.Quantity)
            {
                item.Quantity = mergedQuantity;
                changed = true;
            }
        }

        if (changed)
        {
            if (isNewCart)
            {
                _dbContext.Carts.Add(cart);
            }

            await _dbContext.SaveChangesAsync();
        }

        // The session cart has been consumed; the persistent cart is authoritative.
        Session.Remove(CartKey);
    }

    private async Task<CartResponse> GetPersistentResponseAsync(string userId)
    {
        var cart = await LoadPersistentCartForReadAsync(userId);

        return cart is null ? ToResponse([]) : ToResponseFromCart(cart);
    }

    private async Task<(bool Success, string Message, CartResponse Response)> AddToPersistentCartAsync(
        string userId,
        int productId)
    {
        var cart = await LoadPersistentCartForUpdateAsync(userId);

        if (productId <= 0)
        {
            return (false, "المنتج غير موجود", cart is null ? ToResponse([]) : ToResponseFromCart(cart));
        }

        var existingItem = cart?.Items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is not null)
        {
            if (existingItem.Quantity >= CartRules.MaxQuantityPerItem)
            {
                return (true, "وصلت للحد الأقصى للكمية المسموح بها", ToResponseFromCart(cart!));
            }

            existingItem.Quantity += 1;
            await _dbContext.SaveChangesAsync();

            return (true, "تمت الإضافة للسلة 🛒", ToResponseFromCart(cart!));
        }

        var product = await _dbContext.Products
            .Include(existing => existing.Category)
            .FirstOrDefaultAsync(existing => existing.Id == productId);

        if (product is null)
        {
            return (false, "المنتج غير موجود", cart is null ? ToResponse([]) : ToResponseFromCart(cart));
        }

        if (cart is null)
        {
            cart = new Cart { UserId = userId };
            _dbContext.Carts.Add(cart);
        }

        cart.Items.Add(new CartItem
        {
            Product = product,
            ProductId = product.Id,
            Quantity = 1
        });

        await _dbContext.SaveChangesAsync();

        return (true, "تمت الإضافة للسلة 🛒", ToResponseFromCart(cart));
    }

    private async Task<CartResponse> UpdatePersistentQuantityAsync(string userId, int productId, int quantity)
    {
        var cart = await LoadPersistentCartForUpdateAsync(userId);

        if (cart is null)
        {
            return ToResponse([]);
        }

        // Zero and negative quantities are invalid updates. Deletion must use RemoveAsync.
        if (productId <= 0 || quantity <= 0)
        {
            return ToResponseFromCart(cart);
        }

        var item = cart.Items.FirstOrDefault(existing => existing.ProductId == productId);

        if (item is null)
        {
            return ToResponseFromCart(cart);
        }

        var normalizedQuantity = Math.Min(quantity, CartRules.MaxQuantityPerItem);

        if (item.Quantity != normalizedQuantity)
        {
            item.Quantity = normalizedQuantity;
            await _dbContext.SaveChangesAsync();
        }

        return ToResponseFromCart(cart);
    }

    private async Task<CartResponse> RemoveFromPersistentCartAsync(string userId, int productId)
    {
        var cart = await LoadPersistentCartForUpdateAsync(userId);

        if (cart is null)
        {
            return ToResponse([]);
        }

        var item = productId > 0
            ? cart.Items.FirstOrDefault(existing => existing.ProductId == productId)
            : null;

        if (item is not null)
        {
            cart.Items.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        return ToResponseFromCart(cart);
    }

    private Task<Cart?> LoadPersistentCartForReadAsync(string userId)
    {
        return _dbContext.Carts
            .AsNoTracking()
            .Include(cart => cart.Items)
                .ThenInclude(item => item.Product)
                    .ThenInclude(product => product.Category)
            .FirstOrDefaultAsync(cart => cart.UserId == userId);
    }

    private Task<Cart?> LoadPersistentCartForUpdateAsync(string userId)
    {
        return _dbContext.Carts
            .Include(cart => cart.Items)
                .ThenInclude(item => item.Product)
                    .ThenInclude(product => product.Category)
            .FirstOrDefaultAsync(cart => cart.UserId == userId);
    }

    private async Task<CartResponse> GetSessionResponseAsync()
    {
        var (entries, items, needsSave) = await LoadSessionCartAsync();

        if (needsSave)
        {
            SaveSession(entries);
        }

        return ToResponse(items);
    }

    private async Task<(bool Success, string Message, CartResponse Response)> AddToSessionAsync(int productId)
    {
        var (entries, items, needsSave) = await LoadSessionCartAsync();

        if (productId <= 0)
        {
            if (needsSave)
            {
                SaveSession(entries);
            }

            return (false, "المنتج غير موجود", ToResponse(items));
        }

        var entry = entries.FirstOrDefault(item => item.ProductId == productId);

        if (entry is null)
        {
            var product = await _productService.GetByIdAsync(productId);

            if (product is null)
            {
                if (needsSave)
                {
                    SaveSession(entries);
                }

                return (false, "المنتج غير موجود", ToResponse(items));
            }

            entry = new CartEntry
            {
                ProductId = product.Id,
                Quantity = 1
            };

            entries.Add(entry);
            items.Add(ToCartItemResponse(product, entry.Quantity));
            SaveSession(entries);

            return (true, "تمت الإضافة للسلة 🛒", ToResponse(items));
        }

        if (entry.Quantity >= CartRules.MaxQuantityPerItem)
        {
            if (needsSave)
            {
                SaveSession(entries);
            }

            return (true, "وصلت للحد الأقصى للكمية المسموح بها", ToResponse(items));
        }

        entry.Quantity += 1;
        items.First(item => item.Id == productId).Quantity = entry.Quantity;
        SaveSession(entries);

        return (true, "تمت الإضافة للسلة 🛒", ToResponse(items));
    }

    private async Task<CartResponse> UpdateSessionQuantityAsync(int productId, int quantity)
    {
        var (entries, items, needsSave) = await LoadSessionCartAsync();

        // Zero and negative quantities are invalid updates. Deletion must use RemoveAsync.
        if (productId <= 0 || quantity <= 0)
        {
            if (needsSave)
            {
                SaveSession(entries);
            }

            return ToResponse(items);
        }

        var entry = entries.FirstOrDefault(item => item.ProductId == productId);

        if (entry is null)
        {
            if (needsSave)
            {
                SaveSession(entries);
            }

            return ToResponse(items);
        }

        var normalizedQuantity = Math.Min(quantity, CartRules.MaxQuantityPerItem);

        if (entry.Quantity == normalizedQuantity)
        {
            if (needsSave)
            {
                SaveSession(entries);
            }

            return ToResponse(items);
        }

        entry.Quantity = normalizedQuantity;
        items.First(item => item.Id == productId).Quantity = normalizedQuantity;
        SaveSession(entries);

        return ToResponse(items);
    }

    private async Task<CartResponse> RemoveFromSessionAsync(int productId)
    {
        var (entries, items, needsSave) = await LoadSessionCartAsync();
        var removed = productId > 0
            ? entries.RemoveAll(item => item.ProductId == productId)
            : 0;

        if (removed > 0)
        {
            items.RemoveAll(item => item.Id == productId);
        }

        if (removed > 0 || needsSave)
        {
            SaveSession(entries);
        }

        return ToResponse(items);
    }

    /// <summary>
    /// Loads the stored cart entries, drops invalid or unavailable products, and
    /// hydrates the response from current server-side product data. The returned
    /// needs-save flag indicates that the normalized entries should be written back.
    /// </summary>
    private async Task<(List<CartEntry> Entries, List<CartItemResponse> Items, bool NeedsSave)> LoadSessionCartAsync()
    {
        var (storedEntries, migrated) = ReadStoredEntries();
        var entries = new List<CartEntry>(storedEntries.Count);
        var items = new List<CartItemResponse>(storedEntries.Count);
        var seenProductIds = new HashSet<int>();
        var changed = migrated;

        foreach (var storedEntry in storedEntries)
        {
            if (storedEntry.ProductId <= 0 || storedEntry.Quantity <= 0)
            {
                changed = true;
                continue;
            }

            if (!seenProductIds.Add(storedEntry.ProductId))
            {
                changed = true;
                continue;
            }

            var quantity = Math.Min(storedEntry.Quantity, CartRules.MaxQuantityPerItem);

            if (quantity != storedEntry.Quantity)
            {
                changed = true;
            }

            var product = await _productService.GetByIdAsync(storedEntry.ProductId);

            if (product is null)
            {
                changed = true;
                continue;
            }

            entries.Add(new CartEntry
            {
                ProductId = product.Id,
                Quantity = quantity
            });

            items.Add(ToCartItemResponse(product, quantity));
        }

        var needsSave = changed || !EntriesMatch(storedEntries, entries);

        return (entries, items, needsSave);
    }

    /// <summary>
    /// Reads the compact ID/quantity format and transparently migrates the legacy
    /// format that stored complete product details in the session.
    /// </summary>
    private (List<CartEntry> Entries, bool Migrated) ReadStoredEntries()
    {
        var storedEntries = Session.GetJson<List<CartEntry>>(CartKey);

        if (storedEntries is null || storedEntries.Count == 0)
        {
            return (storedEntries ?? [], false);
        }

        // Legacy carts stored the full CartItemResponse shape. Its "id" property does
        // not map to ProductId, so every legacy entry deserializes with ProductId == 0.
        if (storedEntries.All(entry => entry.ProductId <= 0))
        {
            var legacyItems = Session.GetJson<List<CartItemResponse>>(CartKey) ?? [];

            var migratedEntries = legacyItems
                .Where(item => item.Id > 0)
                .Select(item => new CartEntry
                {
                    ProductId = item.Id,
                    Quantity = item.Quantity
                })
                .ToList();

            return (migratedEntries, true);
        }

        return (storedEntries, false);
    }

    private static CartItemResponse ToCartItemResponse(Product product, int quantity)
    {
        return new CartItemResponse
        {
            Id = product.Id,
            Category = product.Category.Name,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Image = product.Image,
            Quantity = quantity
        };
    }

    private static CartResponse ToResponseFromCart(Cart cart)
    {
        var items = cart.Items
            .Where(item => item.Product is not null)
            .OrderBy(item => item.Id)
            .Select(item => ToCartItemResponse(item.Product, item.Quantity))
            .ToList();

        return ToResponse(items);
    }

    private static bool EntriesMatch(List<CartEntry> storedEntries, List<CartEntry> normalizedEntries)
    {
        if (storedEntries.Count != normalizedEntries.Count)
        {
            return false;
        }

        for (var index = 0; index < storedEntries.Count; index++)
        {
            if (storedEntries[index].ProductId != normalizedEntries[index].ProductId ||
                storedEntries[index].Quantity != normalizedEntries[index].Quantity)
            {
                return false;
            }
        }

        return true;
    }

    private string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.GetCurrentUser()?.Id;
    }

    private ISession Session =>
        _httpContextAccessor.HttpContext?.Session
        ?? throw new InvalidOperationException("No active HTTP session is available.");

    private void SaveSession(List<CartEntry> entries)
    {
        Session.SetJson(CartKey, entries);
    }

    private static CartResponse ToResponse(List<CartItemResponse> items)
    {
        return new CartResponse
        {
            Items = items,
            Count = items.Sum(item => item.Quantity),
            Total = items.Sum(item => item.LineTotal)
        };
    }
}
