﻿namespace Core.TokenService;

public class JwtOptions
{
    public const string Section = "Jwt";

    public string TokenKey { get; set; } = string.Empty;
}
