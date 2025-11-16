using Data.DbContext;
using Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddIdentityCore<User>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<LeanAuditorContext>()
            .AddSignInManager<SignInManager<User>>();

        var jwtOptions = new JwtOptions();
        config.GetSection(JwtOptions.Section).Bind(jwtOptions);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.TokenKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,

                    // Expire token on the exact time (by default token will be still valid up to 5 minutes)
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddScoped<TokenService>();

        return services;
    }
}
