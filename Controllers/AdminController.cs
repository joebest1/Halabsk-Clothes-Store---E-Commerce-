using Halabsk.Net.Models;
using Halabsk.Net.Requests;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("admin/products")]
public class AdminController(ProductService productService) : Controller
{
    private static readonly HashSet<string> AllowedCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "pants",
        "jackets",
        "tshirts"
    };

    [HttpGet("create")]
    public IActionResult CreateProduct()
    {
        return View(new ProductCreateRequest());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(ProductCreateRequest request)
    {
        ValidateProduct(request);

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var product = await productService.CreateAsync(request);
        TempData["AdminMessage"] = $"تمت إضافة {product.Name} بنجاح";

        return RedirectToAction(nameof(CreateProduct));
    }

    private void ValidateProduct(ProductCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Category) ||
            !AllowedCategories.Contains(request.Category.Trim()))
        {
            ModelState.AddModelError(nameof(request.Category), "اختار قسم صحيح");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            ModelState.AddModelError(nameof(request.Name), "اسم المنتج مطلوب");
        }

        if (request.Price <= 0)
        {
            ModelState.AddModelError(nameof(request.Price), "السعر لازم يكون أكبر من صفر");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            ModelState.AddModelError(nameof(request.Description), "الوصف مطلوب");
        }

        if (string.IsNullOrWhiteSpace(request.Image))
        {
            ModelState.AddModelError(nameof(request.Image), "رابط الصورة مطلوب");
        }
    }
}
