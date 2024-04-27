namespace Api.Contracts.Identity.Requests
{
    public class RegisterRequest
    {
        /// <example>John</example>
        public string DisplayName { get; set; }

        /// <example>john@test.com</example>
        public string Email { get; set; }

        /// <example>Pa$$w0rd</example>
        public string Password { get; set; }

        /// <example>john</example>
        public string Username { get; set; }
    }
}
