namespace Api.Contracts.Identity.Dto
{
    public class UserDto
    {
        /// <example>John</example>
        public string DisplayName { get; set; } = null!;

        /// <example>xxxxx.yyyyy.zzzzz</example>
        public string Token { get; set; } = null!;

        /// <example>john</example>
        public string Username { get; set; } = null!;
    }
}
