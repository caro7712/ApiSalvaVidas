#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiSalvarVidas.Models;

[Index("NormalizedEmail", Name = "EmailIndex")]
public partial class AspNetUser : IdentityUser
{
    public string? FotoUrl { get; set; }
}
