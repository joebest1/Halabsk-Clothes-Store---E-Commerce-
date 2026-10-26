using Halabsk.Net.Data;
using Halabsk.Net.Extensions;
using Halabsk.Net.Models;
using Halabsk.Net.Requests;

namespace Halabsk.Net.Services;

public sealed class ContactService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
{
    public async Task SaveAsync(ContactRequest request)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetCurrentUser()?.Id;

        dbContext.ContactMessages.Add(new ContactMessageRecord
        {
            Id = Guid.NewGuid().ToString("N"),
            ContactName = request.ContactName.Trim(),
            ContactEmail = request.ContactEmail.Trim(),
            ContactMessage = request.ContactMessage.Trim(),
            SentAtUtc = DateTime.UtcNow,
            UserId = userId
        });

        await dbContext.SaveChangesAsync();
    }
}
