using System.Security.Claims;
using Halabsk.Net.Models;
using Halabsk.Net.Responses;

namespace Halabsk.Net.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static CurrentUserSession? GetCurrentUser(this ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        if (string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        var roles = principal.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToList();

        return new CurrentUserSession
        {
            Id = id,
            FullName = principal.FindFirstValue(ClaimTypes.Name) ?? email,
            Email = email,
            Role = roles.Contains(UserRoles.Admin, StringComparer.OrdinalIgnoreCase)
                ? UserRoles.Admin
                : roles.FirstOrDefault() ?? UserRoles.Customer
        };
    }
}
