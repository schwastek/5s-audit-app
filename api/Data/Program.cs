using Data.DbContext;
using Data.Menu;
using Data.Options;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Database setup";

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddScoped<SeedDataMenu>();

    // DB
    services.AddDbContext<LeanAuditorContext>();
    services.AddIdentityCore<User>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<LeanAuditorContext>();

    // Configuration (Options pattern)
    services
        .AddOptions<ConnectionStringOptions>()
        .Bind(hostContext.Configuration.GetSection(ConnectionStringOptions.Section));
});

var host = builder.Build();

using (IServiceScope scope = host.Services.CreateScope())
{
    var rootMenu = ActivatorUtilities.CreateInstance<RootMenu>(scope.ServiceProvider);
    await rootMenu.Display();
}
