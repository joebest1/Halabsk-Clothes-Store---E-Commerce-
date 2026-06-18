using Halabsk.Net.Extensions;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("login")]
    [HttpGet("login.html")]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetCurrentUser() is not null)
        {
            return RedirectToAction(nameof(StoreController.Index), "Store");
        }

        return View();
    }

    [HttpGet("signup")]
    [HttpGet("signup.html")]
    public IActionResult Signup()
    {
        if (HttpContext.Session.GetCurrentUser() is not null)
        {
            return RedirectToAction(nameof(StoreController.Index), "Store");
        }

        return View();
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.ClearCurrentUser();
        return RedirectToAction(nameof(StoreController.Index), "Store");
    }
}
