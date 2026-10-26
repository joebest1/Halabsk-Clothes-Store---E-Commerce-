namespace Halabsk.Net.Responses;

public sealed class ApiMessageResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
}
