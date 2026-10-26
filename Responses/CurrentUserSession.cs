using Halabsk.Net.Models;

namespace Halabsk.Net.Responses;

public sealed class CurrentUserSession
{
    public string Id { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = UserRoles.Customer;

    public bool IsAdmin => string.Equals(Role, UserRoles.Admin, StringComparison.OrdinalIgnoreCase);
}
