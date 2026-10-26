using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Dto;

public sealed class LoginRequestDto
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [StringLength(256, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
    [EmailAddress(ErrorMessage = "اكتب بريدًا إلكترونيًا صالحًا")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [StringLength(100, ErrorMessage = "كلمة المرور طويلة جدًا")]
    public string Password { get; init; } = string.Empty;

    public bool RememberMe { get; init; }
}
