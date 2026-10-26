using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Dto;

public sealed class RegisterRequestDto
{
    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(160, MinimumLength = 2, ErrorMessage = "الاسم الكامل يجب أن يكون بين 2 و 160 حرفًا")]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [StringLength(256, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
    [EmailAddress(ErrorMessage = "اكتب بريدًا إلكترونيًا صالحًا")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل")]
    [Compare(nameof(Password), ErrorMessage = "كلمتا المرور غير متطابقتين")]
    public string ConfirmPassword { get; init; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "أوافق على الشروط أولًا")]
    public bool AcceptTerms { get; init; }
}
