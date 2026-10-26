using Halabsk.Net.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

public class AccountController : Controller
{
    [HttpGet("login")]
    [HttpGet("login.html")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.GetCurrentUser() is not null)
        {
            return RedirectAfterAuthentication(returnUrl);
        }

        ViewData["ReturnUrl"] = Url.IsLocalUrl(returnUrl) ? returnUrl : null;
        return View();
    }

    [HttpGet("signup")]
    [HttpGet("signup.html")]
    public IActionResult Signup(string? returnUrl = null)
    {
        if (User.GetCurrentUser() is not null)
        {
            return RedirectAfterAuthentication(returnUrl);
        }

        ViewData["ReturnUrl"] = Url.IsLocalUrl(returnUrl) ? returnUrl : null;
        return View();
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            JwtAuthenticationExtensions.AuthCookieName,
            JwtAuthenticationExtensions.CreateAuthCookieOptions(Request.IsHttps));

        return RedirectToAction(nameof(StoreController.Index), "Store");
    }

    private IActionResult RedirectAfterAuthentication(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction(nameof(StoreController.Index), "Store");
    }
}
