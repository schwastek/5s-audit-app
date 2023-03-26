namespace Api.Data.Menu.Models;

internal abstract class BaseMenu
{
    public abstract Dictionary<ConsoleKey, MenuOption> Options { get; }

    public async Task Display()
    {
        // Title
        Console.WriteLine("Menu:");
        Console.WriteLine();

        // Display options
        foreach (var option in Options.Values)
        {
            Console.WriteLine(option.DisplayName);
        }

        // Choose option
        Console.WriteLine();
        Console.Write("Choose option: ");

        // Validate input
        ConsoleKey input;
        while (!IsValidInput(out input))
        {
            Console.WriteLine();
            Console.WriteLine("Invalid input. Try again...");
            Console.WriteLine();
            Console.Write("Choose option: ");
        }

        // Invoke selected option's action
        Console.WriteLine(Environment.NewLine, Environment.NewLine);
        await Options[input].Action.Invoke();
    }

    private bool IsValidInput(out ConsoleKey input)
    {
        var keyStroke = Console.ReadKey();
        input = keyStroke.Key;

        if (Options.ContainsKey(keyStroke.Key))
        {
            input = keyStroke.Key;
            return true;
        }

        return false;
    }
}
