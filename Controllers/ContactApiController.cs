using Halabsk.Net.Models;
using Halabsk.Net.Services;
using Microsoft.AspNetCore.Mvc;

namespace Halabsk.Net.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactApiController : ControllerBase
{
    private readonly ContactService _contactService;

    public ContactApiController(ContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] ContactRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ContactName) ||
            string.IsNullOrWhiteSpace(request.ContactEmail) ||
            string.IsNullOrWhiteSpace(request.ContactMessage))
        {
            return BadRequest(new ApiMessageResponse
            {
                Success = false,
                Message = "اكمل البيانات"
            });
        }

        await _contactService.SaveAsync(request);

        return Ok(new ApiMessageResponse
        {
            Success = true,
            Message = "تم الإرسال بنجاح ✅"
        });
    }
}
