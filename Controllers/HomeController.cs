using System.Diagnostics;
using Halabsk.Net.Models;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction(nameof(StoreController.Index), "Store");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
