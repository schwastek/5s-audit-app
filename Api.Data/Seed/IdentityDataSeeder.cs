using Api.Core.Domain;
using Api.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Seed;

public static class IdentityDataSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager)
    {
        List<User> users = new List<User>
            {
                new User { DisplayName = "John", UserName = "john", Email = "john@test.com" }
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
            context.Database.ExecuteSqlRaw($"DELETE FROM {table}");
        }

        await context.SaveChangesAsync();
    }
}
