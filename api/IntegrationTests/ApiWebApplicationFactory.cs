using Data.DbContext;
using IntegrationTests.AuthHandlers;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // The test app's `builder.ConfigureServices` callback is executed
        // after the app's Program.cs code is executed
        builder
            // Set SUT's environment
            .UseEnvironment(Environments.Development)
            .ConfigureServices(services =>
        {
            // Mock authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandlerConstants.AuthenticationScheme;
                })
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
                    TestAuthHandlerConstants.AuthenticationScheme,
                    options => { }
                );

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<LeanAuditorContext>();
                var logger = scopedServices.GetRequiredService<ILogger<ApiWebApplicationFactory>>();

                // Seed DB
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                try
                {
                    Utilities.InitializeDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }
}
