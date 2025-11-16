namespace Infrastructure.OrderByService;

public interface IOrderByQuery
{
    public string OrderBy { get; init; }
}

/// <summary>
/// Helper class for mapping from order by request to order by query.
/// </summary>
public class OrderByQuery : IOrderByQuery
{
    public const string OrderByDefault = "author asc";

    public string OrderBy { get; init; }

    public OrderByQuery(string? orderBy)
    {
        OrderBy = orderBy ?? OrderByDefault;
    }
}
