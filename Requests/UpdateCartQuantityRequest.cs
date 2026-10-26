using System.ComponentModel.DataAnnotations;
using Halabsk.Net.Models;

namespace Halabsk.Net.Requests;

public sealed class UpdateCartQuantityRequest
{
    [Range(1, CartRules.MaxQuantityPerItem, ErrorMessage = "الكمية لازم تكون عددًا صحيحًا أكبر من صفر")]
    public int Quantity { get; init; }
}
