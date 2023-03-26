using Microsoft.Extensions.Hosting;

namespace Api.Data.Menu;

internal class RootMenu
{
    private readonly IHostEnvironment _environment;
    private readonly SeedDataMenu _seedSampleDataMenu;

    public RootMenu(IHostEnvironment environment,
        SeedDataMenu seedSampleDataMenu)
    {
        _environment = environment;
        _seedSampleDataMenu = seedSampleDataMenu;
    }

    public async Task Display()
    {
        // Current environment
        Console.WriteLine("Environment: {0}", _environment.EnvironmentName);
        Console.WriteLine();

        // Display menu
        try
        {
            await _seedSampleDataMenu.Display();
        } catch (Exception e)
        {
            Console.WriteLine("Error!");
            Console.WriteLine(e.Message);
        } finally
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
