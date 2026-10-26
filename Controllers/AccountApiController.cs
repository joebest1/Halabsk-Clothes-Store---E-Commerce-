using Halabsk.Net.Configuration;
using Halabsk.Net.Dto;
using Halabsk.Net.Extensions;
using Halabsk.Net.Models;
using Halabsk.Net.Responses;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Halabsk.Net.Controllers;

[ApiController]
[Route("api/account")]
public class AccountApiController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtTokenService _jwtTokenService;
    private readonly CartService _cartService;
    private readonly JwtOptions _jwtOptions;

    public AccountApiController(
        UserService userService,
        JwtTokenService jwtTokenService,
        CartService cartService,
        IOptions<JwtOptions> jwtOptions)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _cartService = cartService;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "اكمل البيانات"
            });
        }

        var user = await _userService.ValidateAsync(request.Email, request.Password);

        if (user is null)
        {
            return Unauthorized(new ApiMessageResponse
            {
                Success = false,
                Message = "الإيميل أو الباسورد غلط"
            });
        }

        // The login request still has the anonymous session available here.
        // Merge it into the user's persistent cart before issuing the auth cookie.
        await _cartService.MergeSessionCartAsync(user.Id);

        var roles = await _userService.GetRolesAsync(user);
        var (token, expiresAtUtc) = _jwtTokenService.CreateToken(user, roles);

        // RememberMe controls only how long the browser keeps the auth cookie.
        // The access token itself always expires after Jwt:AccessTokenMinutes.
        DateTimeOffset? cookieExpiresAtUtc = request.RememberMe
            ? DateTimeOffset.UtcNow.AddDays(_jwtOptions.RememberMeDays)
            : null;

        Response.Headers.CacheControl = "no-store";
        Response.Cookies.Append(
            JwtAuthenticationExtensions.AuthCookieName,
            token,
            JwtAuthenticationExtensions.CreateAuthCookieOptions(
                Request.IsHttps,
                cookieExpiresAtUtc));

        return Ok(new AuthResponse
        {
            Success = true,
            Message = $"أهلا {user.FullName} 👋",
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            User = new CurrentUserSession
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = roles.Contains(UserRoles.Admin, StringComparer.OrdinalIgnoreCase)
                    ? UserRoles.Admin
                    : roles.FirstOrDefault() ?? UserRoles.Customer
            }
        });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.ConfirmPassword))
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "اكمل البيانات"
            });
        }

        if (!request.AcceptTerms)
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "وافق على الشروط أولًا"
            });
        }

        if (request.Password.Length < 8)
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "كلمة المرور يجب أن تكون 8 أحرف على الأقل"
            });
        }

        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "كلمتا المرور غير متطابقتين"
            });
        }

        var result = await _userService.RegisterAsync(request);

        if (!result.Success)
        {
            return Conflict(new ApiMessageResponse
            {
                Success = false,
                Message = result.Message
            });
        }

        return Ok(new ApiMessageResponse
        {
            Success = true,
            Message = result.Message
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var currentUser = User.GetCurrentUser();

        if (currentUser is null)
        {
            return Unauthorized(new ApiMessageResponse
            {
                Success = false,
                Message = "سجل الدخول الأول"
            });
        }

        return Ok(currentUser);
    }
}
