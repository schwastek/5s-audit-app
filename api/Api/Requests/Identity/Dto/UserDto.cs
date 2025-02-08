namespace Api.Requests.Identity.Dto;

public sealed record UserDto
{
    /// <example>John</example>
    public string DisplayName { get; set; } = string.Empty;

    /// <example>xxxxx.yyyyy.zzzzz</example>
    public string Token { get; set; } = string.Empty;

    /// <example>john</example>
    public string Username { get; set; } = string.Empty;
}
