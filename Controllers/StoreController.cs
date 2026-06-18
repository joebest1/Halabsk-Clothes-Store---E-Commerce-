using Halabsk.Net.Models;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

public class StoreController : Controller
{
    private readonly ProductService _productService;

    public StoreController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("/")]
    [HttpGet("index")]
    [HttpGet("index.html")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("products")]
    [HttpGet("products.html")]
    public async Task<IActionResult> Products()
    {
        var previewProducts = (await _productService.GetAllAsync())
            .Take(4)
            .ToList();

        return View(new ProductsPageViewModel
        {
            PreviewProducts = previewProducts
        });
    }

    [HttpGet("pants")]
    [HttpGet("pants.html")]
    public IActionResult Pants()
    {
        return View("Category", BuildCategoryPage(
            title: "البناطيل",
            tag: "Pants",
            heading: "البناطيل",
            category: "pants",
            searchPlaceholder: "ابحث عن بنطلون..."));
    }

    [HttpGet("jacket")]
    [HttpGet("jacket.html")]
    public IActionResult Jacket()
    {
        return View("Category", BuildCategoryPage(
            title: "الجاكيتات",
            tag: "Jackets",
            heading: "الجاكيتات",
            category: "jackets",
            searchPlaceholder: "ابحث عن جاكيت..."));
    }

    [HttpGet("tshirt")]
    [HttpGet("tshirt.html")]
    public IActionResult Tshirt()
    {
        return View("Category", BuildCategoryPage(
            title: "التيشيرتات",
            tag: "T-Shirts",
            heading: "التيشيرتات",
            category: "tshirts",
            searchPlaceholder: "ابحث عن تيشيرت..."));
    }

    [HttpGet("contact")]
    [HttpGet("contact.html")]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet("cart")]
    [HttpGet("cart.html")]
    public IActionResult Cart()
    {
        return View();
    }

    private static CategoryPageViewModel BuildCategoryPage(
        string title,
        string tag,
        string heading,
        string category,
        string searchPlaceholder)
    {
        return new CategoryPageViewModel
        {
            Title = title,
            Tag = tag,
            Heading = heading,
            Category = category,
            SearchPlaceholder = searchPlaceholder
        };
    }
}
