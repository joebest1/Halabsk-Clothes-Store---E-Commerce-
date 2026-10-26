using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Requests;

public sealed class ProductCreateRequest
{
    [Required(ErrorMessage = "اختار قسم صحيح")]
    [StringLength(64, ErrorMessage = "اسم القسم طويل جدًا")]
    public string Category { get; init; } = string.Empty;

    [Required(ErrorMessage = "اسم المنتج مطلوب")]
    [StringLength(160, ErrorMessage = "اسم المنتج طويل جدًا")]
    public string Name { get; init; } = string.Empty;

    [Range(0.01, 999999.99, ErrorMessage = "السعر لازم يكون أكبر من صفر")]
    public decimal Price { get; init; }

    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(500, ErrorMessage = "الوصف طويل جدًا")]
    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = "رابط الصورة مطلوب")]
    [StringLength(1000, ErrorMessage = "رابط الصورة طويل جدًا")]
    [Url(ErrorMessage = "رابط الصورة غير صالح")]
    public string Image { get; init; } = string.Empty;
}
