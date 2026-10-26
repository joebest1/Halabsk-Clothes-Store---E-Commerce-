using System.ComponentModel.DataAnnotations;

namespace Halabsk.Net.Requests;

public sealed class AddToCartRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "بيانات المنتج غير صالحة")]
    public int ProductId { get; init; }
}
