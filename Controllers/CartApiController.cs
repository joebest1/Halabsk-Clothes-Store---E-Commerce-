using Halabsk.Net.Models;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

[ApiController]
[Route("api/cart")]
public class CartApiController : ControllerBase
{
    private readonly CartService _cartService;

    public CartApiController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public Task<CartResponse> Get()
    {
        return _cartService.GetResponseAsync();
    }

    [HttpPost("items")]
    public async Task<IActionResult> Add([FromBody] AddToCartRequest request)
    {
        if (request.ProductId <= 0)
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "بيانات المنتج غير صالحة"
            });
        }

        var result = await _cartService.AddAsync(request.ProductId);

        if (!result.Success)
        {
            return NotFound(new
            {
                success = false,
                message = result.Message,
                cart = result.Response
            });
        }

        return Ok(new
        {
            success = true,
            message = result.Message,
            cart = result.Response
        });
    }

    [HttpPatch("items/{productId:int}")]
    public async Task<IActionResult> UpdateQuantity(int productId, [FromBody] UpdateCartQuantityRequest request)
    {
        var cart = await _cartService.UpdateQuantityAsync(productId, request.Quantity);

        return Ok(new
        {
            success = true,
            cart
        });
    }

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> Remove(int productId)
    {
        var cart = await _cartService.RemoveAsync(productId);

        return Ok(new
        {
            success = true,
            cart
        });
    }
}
