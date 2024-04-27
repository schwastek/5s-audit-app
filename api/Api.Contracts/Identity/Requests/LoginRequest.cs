namespace Api.Contracts.Identity.Requests
{
    public class LoginRequest
    {
        /// <example>john@test.com</example>
        public string Email { get; set; }

        /// <example>Pa$$w0rd</example>
        public string Password { get; set; }
    }
}
