using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Api.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Api.IntegrationTests.Helpers;
using Api.IntegrationTests.AuthHandlers;

namespace Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // The test app's `builder.ConfigureServices` callback is executed
            // after the app's `Startup.ConfigureServices` code is executed
            builder.ConfigureServices(services =>
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
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Seed DB
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

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
}
