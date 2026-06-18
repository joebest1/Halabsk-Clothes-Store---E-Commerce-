namespace Halabsk.Net.Models;

public sealed class ContactRequest
{
    public string ContactName { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public string ContactMessage { get; init; } = string.Empty;
}

public sealed class ContactMessageRecord
{
    public string Id { get; init; } = string.Empty;
    public string ContactName { get; init; } = string.Empty;
    public string ContactEmail { get; init; } = string.Empty;
    public string ContactMessage { get; init; } = string.Empty;
    public DateTime SentAtUtc { get; init; }
}
