namespace Infrastructure.OrderByService;

public record OrderByParameter
{
    public string PropertyName { get; init; } = string.Empty;
    public bool SortDescending { get; init; }
}
