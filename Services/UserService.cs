using Halabsk.Net.Dto;
using Halabsk.Net.Models;
using Microsoft.AspNetCore.Identity;

namespace Halabsk.Net.Services;

public sealed class UserService(UserManager<ApplicationUser> userManager)
{
    public async Task<ApplicationUser?> ValidateAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(NormalizeEmail(email));

        if (user is null || await userManager.IsLockedOutAsync(user))
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            await userManager.AccessFailedAsync(user);
            return null;
        }

        if (user.AccessFailedCount > 0)
        {
            await userManager.ResetAccessFailedCountAsync(user);
        }

        return user;
    }

    public async Task<(bool Success, string Message, ApplicationUser? User)> RegisterAsync(RegisterRequestDto request)
    {
        var normalizedEmail = NormalizeEmail(request.Email);

        if (await userManager.FindByEmailAsync(normalizedEmail) is not null)
        {
            return (false, "الإيميل مستخدم بالفعل", null);
        }

        var user = new ApplicationUser
        {
            UserName = normalizedEmail,
            Email = normalizedEmail,
            EmailConfirmed = true,
            FullName = request.FullName.Trim()
        };

        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return (false, TranslateErrors(createResult), null);
        }

        var roleResult = await userManager.AddToRoleAsync(user, UserRoles.Customer);

        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return (false, TranslateErrors(roleResult), null);
        }

        return (true, "تم إنشاء الحساب 🎉", user);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await userManager.GetRolesAsync(user);
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static string TranslateErrors(IdentityResult result)
    {
        var errors = result.Errors.Select(error => error.Code switch
        {
            nameof(IdentityErrorDescriber.DuplicateEmail) => "الإيميل مستخدم بالفعل",
            nameof(IdentityErrorDescriber.DuplicateUserName) => "الإيميل مستخدم بالفعل",
            nameof(IdentityErrorDescriber.PasswordTooShort) => "كلمة المرور قصيرة جدًا",
            nameof(IdentityErrorDescriber.PasswordRequiresDigit) => "كلمة المرور لازم تحتوي على رقم",
            nameof(IdentityErrorDescriber.PasswordRequiresLower) => "كلمة المرور لازم تحتوي على حرف صغير",
            nameof(IdentityErrorDescriber.PasswordRequiresUpper) => "كلمة المرور لازم تحتوي على حرف كبير",
            nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric) => "كلمة المرور لازم تحتوي على رمز",
            nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars) => "كلمة المرور لازم تحتوي على أحرف مختلفة",
            _ => error.Description
        });

        return string.Join(" ", errors);
    }
}
