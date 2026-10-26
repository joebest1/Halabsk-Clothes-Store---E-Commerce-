using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Halabsk.Net.Models;

// The CLR namespace intentionally remains Halabsk.Net.Models so the existing
// EF Core migrations and model snapshot keep the same entity type names.
public sealed class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(160)]
    public string FullName { get; set; } = string.Empty;
}
