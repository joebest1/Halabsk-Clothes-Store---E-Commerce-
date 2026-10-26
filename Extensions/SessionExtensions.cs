using System.Text.Json;
using Halabsk.Net.Models;

namespace Halabsk.Net.Extensions;

public static class SessionExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static void SetJson<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value, JsonOptions));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var rawValue = session.GetString(key);
        return string.IsNullOrWhiteSpace(rawValue)
            ? default
            : JsonSerializer.Deserialize<T>(rawValue, JsonOptions);
    }

    public static int GetCartCount(this ISession session)
    {
        // Works for both the compact { productId, quantity } format and legacy
        // carts that stored full item details, because both include "quantity".
        var cart = session.GetJson<List<CartEntry>>("Cart") ?? [];
        return cart.Sum(item => Math.Clamp(item.Quantity, 0, CartRules.MaxQuantityPerItem));
    }
}
