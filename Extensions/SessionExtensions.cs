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

    public static CurrentUserSession? GetCurrentUser(this ISession session)
    {
        return session.GetJson<CurrentUserSession>("CurrentUser");
    }

    public static void SetCurrentUser(this ISession session, CurrentUserSession user)
    {
        session.SetJson("CurrentUser", user);
    }

    public static void ClearCurrentUser(this ISession session)
    {
        session.Remove("CurrentUser");
    }

    public static int GetCartCount(this ISession session)
    {
        var cart = session.GetJson<List<CartItem>>("Cart") ?? [];
        return cart.Sum(item => item.Quantity);
    }
}
