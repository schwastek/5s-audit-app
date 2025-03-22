using Data.DbContext;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Seed;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager)
    {
        var users = new List<User>
            {
                new() { DisplayName = "John", UserName = "john", Email = "john@test.com" }
            };

        foreach (User user in users)
        {
            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }

    public static async Task ClearAsync(LeanAuditorContext context)
    {
        var tables = new[]
        {
            "AspNetRoleClaims",
            "AspNetRoles",
            "AspNetUserClaims",
            "AspNetUserLogins",
            "AspNetUserRoles",
            "AspNetUserTokens",
            "RefreshToken",
            "AspNetUsers"
        };

        foreach (var table in tables)
        {
#pragma warning disable EF1002 // Risk of vulnerability to SQL injection.
            context.Database.ExecuteSqlRaw($"DELETE FROM {table}");
#pragma warning restore EF1002 // Risk of vulnerability to SQL injection.
        }

        await context.SaveChangesAsync();
    }
}
