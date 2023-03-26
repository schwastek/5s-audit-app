namespace Api.Data.Menu.Models;

internal readonly record struct MenuOption(string DisplayName, Func<Task> Action);
