namespace Halabsk.Net.Responses;

public sealed class AuthResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
    public CurrentUserSession User { get; init; } = new();
}
