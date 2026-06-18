using Halabsk.Net.Extensions;
using Halabsk.Net.Models;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

[ApiController]
[Route("api/account")]
public class AccountApiController : ControllerBase
{
    private readonly UserService _userService;

    public AccountApiController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
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

        HttpContext.Session.SetCurrentUser(new CurrentUserSession
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email
        });

        return Ok(new ApiMessageResponse
        {
            Success = true,
            Message = $"أهلا {user.FullName} 👋"
        });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
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
}
