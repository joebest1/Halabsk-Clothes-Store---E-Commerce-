using Halabsk.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsApiController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsApiController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? category)
    {
        var products = string.IsNullOrWhiteSpace(category)
            ? await _productService.GetAllAsync()
            : await _productService.GetByCategoryAsync(category);

        return Ok(products);
    }
}
