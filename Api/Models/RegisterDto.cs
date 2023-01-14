using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class RegisterDto
{
    /// <example>John</example>
    [Required]
    public string DisplayName { get; set; }

    /// <example>john@test.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <example>Pa$$w0rd</example>
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password is too weak")]
    public string Password { get; set; }

    /// <example>john</example>
    [Required]
    public string Username { get; set; }
}
