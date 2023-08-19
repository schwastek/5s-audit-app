namespace Api.Mappers.OrderBy;

public record Sortable
{
    public string PropertyName { get; init; } = string.Empty;
    public bool SortDescending { get; init; }
}
