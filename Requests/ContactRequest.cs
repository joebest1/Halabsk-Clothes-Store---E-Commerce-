using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Requests;

public sealed class ContactRequest
{
    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(160, ErrorMessage = "الاسم طويل جدًا")]
    public string ContactName { get; init; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [StringLength(256, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
    [EmailAddress(ErrorMessage = "اكتب بريدًا إلكترونيًا صالحًا")]
    public string ContactEmail { get; init; } = string.Empty;

    [Required(ErrorMessage = "الرسالة مطلوبة")]
    [StringLength(2000, ErrorMessage = "الرسالة طويلة جدًا")]
    public string ContactMessage { get; init; } = string.Empty;
}
