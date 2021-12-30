using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password is too weak")]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
