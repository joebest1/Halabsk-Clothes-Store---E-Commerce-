using Halabsk.Net.Extensions;
using Halabsk.Net.Models;

namespace Halabsk.Net.Services;

public sealed class CartService
{
    private const string CartKey = "Cart";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ProductService _productService;

    public CartService(IHttpContextAccessor httpContextAccessor, ProductService productService)
    {
        _httpContextAccessor = httpContextAccessor;
        _productService = productService;
    }

    public Task<CartResponse> GetResponseAsync()
    {
        var cart = Session.GetJson<List<CartItem>>(CartKey) ?? [];
        return Task.FromResult(ToResponse(cart));
    }

    public async Task<(bool Success, string Message, CartResponse Response)> AddAsync(int productId)
    {
        var product = await _productService.GetByIdAsync(productId);

        if (product is null)
        {
            return (false, "المنتج غير موجود", await GetResponseAsync());
        }

        var cart = Session.GetJson<List<CartItem>>(CartKey) ?? [];
        var existingItem = cart.FirstOrDefault(item => item.Id == productId);

        if (existingItem is null)
        {
            cart.Add(new CartItem
            {
                Id = product.Id,
                Category = product.Category,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image,
                Quantity = 1
            });
        }
        else
        {
            existingItem.Quantity += 1;
        }

        Save(cart);
        return (true, "تمت الإضافة للسلة 🛒", ToResponse(cart));
    }

    public Task<CartResponse> UpdateQuantityAsync(int productId, int quantity)
    {
        var cart = Session.GetJson<List<CartItem>>(CartKey) ?? [];
        var item = cart.FirstOrDefault(cartItem => cartItem.Id == productId);

        if (item is not null)
        {
            if (quantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            Save(cart);
        }

        return Task.FromResult(ToResponse(cart));
    }

    public Task<CartResponse> RemoveAsync(int productId)
    {
        var cart = Session.GetJson<List<CartItem>>(CartKey) ?? [];
        cart.RemoveAll(item => item.Id == productId);
        Save(cart);

        return Task.FromResult(ToResponse(cart));
    }

    private ISession Session =>
        _httpContextAccessor.HttpContext?.Session
        ?? throw new InvalidOperationException("No active HTTP session is available.");

    private void Save(List<CartItem> cart)
    {
        Session.SetJson(CartKey, cart);
    }

    private static CartResponse ToResponse(List<CartItem> cart)
    {
        return new CartResponse
        {
            Items = cart,
            Count = cart.Sum(item => item.Quantity),
            Total = cart.Sum(item => item.LineTotal)
        };
    }
}
