using IntegrationTests.AuthHandlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        });
    }
}
