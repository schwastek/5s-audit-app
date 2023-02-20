namespace Api.Core.Options;

public class ConnectionStringOptions
{
    public const string Section = "ConnectionStrings";

    public string DefaultConnection { get; set; } = string.Empty;
}