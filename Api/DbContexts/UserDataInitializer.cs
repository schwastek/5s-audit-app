using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain;
using Microsoft.AspNetCore.Identity;

namespace Api.DbContexts
{
    public class UserDataInitializer
    {
        public static async Task Seed(UserManager<User> userManager)
        {
            if (userManager.Users.Any())
            {
                // DB has been seeded
                return;
            }

            List<User> users = new List<User>
                {
                    new User { DisplayName = "John", UserName = "john", Email = "john@test.com" }
                };

            foreach (User user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
