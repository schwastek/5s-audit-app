using Data.DbContext;
using Data.Menu.Models;
using Data.Seed;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Menu;

internal class SeedDataMenu : BaseMenu
{
    private readonly LeanAuditorContext _context;
    private readonly UserManager<User> _userManager;

    public override Dictionary<ConsoleKey, MenuOption> Options { get; }

    public SeedDataMenu(LeanAuditorContext context,
        UserManager<User> userManager)
    {
        Options = new()
        {
            { ConsoleKey.D1, new MenuOption("1. Reset and seed sample data", SeedSampleData) },
            { ConsoleKey.D2, new MenuOption("2. Reset and seed identity data", SeedIdentityData) }
        };
        _context = context;
        _userManager = userManager;
    }

    private async Task SeedSampleData()
    {
        await MigrateAsync();

        Console.WriteLine("Clearing data...");
        await SampleDataSeeder.ClearAsync(_context);

        Console.WriteLine("Seeding data...");
        await SampleDataSeeder.SeedAsync(_context);

        Console.WriteLine("Done seeding data!");
    }

    private async Task SeedIdentityData()
    {
        await MigrateAsync();

        Console.WriteLine("Clearing identity data...");
        await IdentityDataSeeder.ClearAsync(_context);
        
        Console.WriteLine("Seeding identity data...");
        await IdentityDataSeeder.SeedAsync(_userManager);

        Console.WriteLine("Done seeding identity data!");
    }

    private async Task MigrateAsync()
    {
        // Display pending migrations
        var migrations = _context.Database.GetPendingMigrations().ToList();
        Console.WriteLine("Applying migrations...");
        Console.WriteLine("Pending migrations: {0}", migrations.Count);
        migrations.ToList().ForEach(Console.WriteLine);

        // Apply migrations
        await _context.Database.MigrateAsync();
    }
}
