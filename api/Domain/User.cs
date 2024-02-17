using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain;

public class User : IdentityUser
{
    public string? DisplayName { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
