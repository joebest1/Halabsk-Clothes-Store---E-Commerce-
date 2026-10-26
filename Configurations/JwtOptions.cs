namespace Halabsk.Net.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "Halabsk.Net";
    public string Audience { get; init; } = "Halabsk.Net";
    public string Key { get; init; } = string.Empty;
    public int AccessTokenMinutes { get; init; } = 60;
    public int RememberMeDays { get; init; } = 7;
}
