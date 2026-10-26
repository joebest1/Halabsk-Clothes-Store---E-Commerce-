using System.Security.Claims;
using System.Text;
using Halabsk.Net.Configuration;
using Halabsk.Net.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Halabsk.Net.Extensions;

public static class JwtAuthenticationExtensions
{
    public const string AuthCookieName = "halabsk_access_token";

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = jwtSection.Get<JwtOptions>()
            ?? throw new InvalidOperationException("The Jwt configuration section is missing.");

        var keyConfigurationHelp =
            $"Jwt:Key must be configured with at least 32 characters. " +
            $"Current environment: {environment.EnvironmentName}. " +
            "For local development, store the key with " +
            "`dotnet user-secrets set \"Jwt:Key\" \"<at-least-32-character-secret>\"` " +
            "and start with `dotnet run` so ASPNETCORE_ENVIRONMENT=Development. " +
            "For production, set the Jwt__Key environment variable.";

        if (string.IsNullOrWhiteSpace(jwtOptions.Key) ||
            Encoding.UTF8.GetByteCount(jwtOptions.Key) < 32)
        {
            
            throw new InvalidOperationException(keyConfigurationHelp);
        }

        if (string.IsNullOrWhiteSpace(jwtOptions.Issuer) ||
            string.IsNullOrWhiteSpace(jwtOptions.Audience))
        {
            throw new InvalidOperationException(
                "Jwt:Issuer and Jwt:Audience must be configured.");
        }

        services.AddOptions<JwtOptions>()
            .Bind(jwtSection)
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Key) &&
                           Encoding.UTF8.GetByteCount(options.Key) >= 32,
                keyConfigurationHelp)
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Issuer),
                "Jwt:Issuer is required.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Audience),
                "Jwt:Audience is required.")
            .Validate(
                options => options.AccessTokenMinutes is >= 5 and <= 1440,
                "Jwt:AccessTokenMinutes must be between 5 and 1440 minutes.")
            .Validate(
                options => options.RememberMeDays is >= 1 and <= 365,
                "Jwt:RememberMeDays must be between 1 and 365 days.")
            .ValidateOnStart();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (string.IsNullOrEmpty(context.Token) &&
                            context.Request.Cookies.TryGetValue(AuthCookieName, out var cookieToken))
                        {
                            context.Token = cookieToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        if (context.Response.HasStarted)
                        {
                            return;
                        }

                        context.HandleResponse();

                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.Headers.Append("WWW-Authenticate", "Bearer");
                            context.Response.ContentType = "application/json; charset=utf-8";
                            await context.Response.WriteAsJsonAsync(new ApiMessageResponse
                            {
                                Success = false,
                                Message = "سجل الدخول الأول"
                            });
                            return;
                        }

                        var returnUrl = context.Request.Method == HttpMethods.Get
                            ? context.Request.Path + context.Request.QueryString
                            : "/";

                        context.Response.Redirect(
                            $"/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
                    },
                    OnForbidden = async context =>
                    {
                        if (context.Response.HasStarted)
                        {
                            return;
                        }

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;

                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.ContentType = "application/json; charset=utf-8";
                            await context.Response.WriteAsJsonAsync(new ApiMessageResponse
                            {
                                Success = false,
                                Message = "مش مسموح لك بالوصول"
                            });
                            return;
                        }

                        context.Response.ContentType = "text/plain; charset=utf-8";
                        await context.Response.WriteAsync("الصفحة دي للأدمن فقط");
                    }
                };
            });

        return services;
    }

    public static CookieOptions CreateAuthCookieOptions(bool secure, DateTimeOffset? expiresAtUtc = null)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = expiresAtUtc
        };
    }
}
