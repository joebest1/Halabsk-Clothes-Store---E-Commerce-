using System.Text.Json;
using Halabsk.Net.Models;

namespace Halabsk.Net.Services;

public sealed class UserService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public UserService(IWebHostEnvironment environment)
    {
        _filePath = Path.Combine(environment.ContentRootPath, "Data", "users.json");
        EnsureFileExists();
    }

    public async Task<UserRecord?> ValidateAsync(string email, string password)
    {
        await _lock.WaitAsync();

        try
        {
            var users = await ReadUsersUnsafeAsync();
            return users.FirstOrDefault(user =>
                string.Equals(user.Email, email.Trim(), StringComparison.OrdinalIgnoreCase) &&
                user.Password == password);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<(bool Success, string Message, UserRecord? User)> RegisterAsync(SignupRequest request)
    {
        await _lock.WaitAsync();

        try
        {
            var users = await ReadUsersUnsafeAsync();

            if (users.Any(user => string.Equals(user.Email, request.Email.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "الإيميل مستخدم بالفعل", null);
            }

            var user = new UserRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                Password = request.Password
            };

            users.Add(user);
            await WriteUsersUnsafeAsync(users);

            return (true, "تم إنشاء الحساب 🎉", user);
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

    private async Task<List<UserRecord>> ReadUsersUnsafeAsync()
    {
        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<UserRecord>>(stream, _jsonOptions) ?? [];
    }

    private async Task WriteUsersUnsafeAsync(List<UserRecord> users)
    {
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, users, _jsonOptions);
    }
}
