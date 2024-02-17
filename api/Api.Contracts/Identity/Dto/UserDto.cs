namespace Api.Contracts.Identity.Dto
{
    public class UserDto
    {
        /// <example>John</example>
        public string? DisplayName { get; set; }

        /// <example>xxxxx.yyyyy.zzzzz</example>
        public string? Token { get; set; }

        /// <example>john</example>
        public string? Username { get; set; }
    }
}
