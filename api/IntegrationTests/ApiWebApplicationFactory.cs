using Data.Context;
using IntegrationTests.AuthHandlers;
using IntegrationTests.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // The test app's `builder.ConfigureServices` callback is executed
        // after the app's Program.cs code is executed
        builder
            .UseEnvironment(Environments.Development)
            .ConfigureServices(services =>
            {
                MockAuthentication(services);
                ReplaceCurrentUserService(services);
            });
    }

    private static void MockAuthentication(IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandlerConstants.AuthenticationScheme;
            })
            .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
                TestAuthHandlerConstants.AuthenticationScheme,
                options => { }
            );
    }

    private static void ReplaceCurrentUserService(IServiceCollection services)
    {
        // Remove existing registration
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICurrentUserService));
        if (descriptor is not null) services.Remove(descriptor);

        // Add test implementation
        services.AddSingleton<ICurrentUserService, TestCurrentUserService>();
    }
}
