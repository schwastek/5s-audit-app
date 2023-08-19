namespace Api.Mappers.OrderBy;

public record OrderByParameter
{
    public string PropertyName { get; init; } = string.Empty;
    public bool SortDescending { get; init; }
}
