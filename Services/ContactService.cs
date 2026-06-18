using System.Text.Json;
using Halabsk.Net.Models;

namespace Halabsk.Net.Services;

public sealed class ContactService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public ContactService(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(environment.ContentRootPath, "Data", "contact-messages.json");
        EnsureFileExists();
    }

    public async Task SaveAsync(ContactRequest request)
    {
        await _lock.WaitAsync();

        try
        {
            var messages = await ReadMessagesUnsafeAsync();

            messages.Add(new ContactMessageRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                ContactName = request.ContactName.Trim(),
                ContactEmail = request.ContactEmail.Trim(),
                ContactMessage = request.ContactMessage.Trim(),
                SentAtUtc = DateTime.UtcNow
            });

            await using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, messages, _jsonOptions);
        }
        finally
        {
            _lock.Release();
        }
    }

    private void EnsureFileExists()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<ContactMessageRecord>> ReadMessagesUnsafeAsync()
    {
        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<ContactMessageRecord>>(stream, _jsonOptions) ?? [];
    }
}
