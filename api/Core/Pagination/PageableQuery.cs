namespace Core.Pagination;

public interface IPageableQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

/// <summary>
/// Helper class for mapping from pageable request to pageable query.
/// </summary>
public class PageableQuery : IPageableQuery
{
    // Don't let the user pass page number -1 in the URL params
    public const int PageNumberDefault = 1;
    public const int PageSizeMin = 1;
    public const int PageSizeMax = 20;
    public const int PageSizeDefault = 5;

    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public PageableQuery(int? pageNumber, int? pageSize)
    {
        PageNumber = GetPageNumber(pageNumber);
        PageSize = GetPageSize(pageSize);
    }

    private static int GetPageNumber(int? currentPageNumber = null)
    {
        int pageNumber = currentPageNumber ?? PageNumberDefault;
        pageNumber = pageNumber < PageNumberDefault ? PageNumberDefault : pageNumber;

        return pageNumber;
    }

    private static int GetPageSize(int? currentPageSize = null)
    {
        int pageSize = currentPageSize ?? PageSizeDefault;
        pageSize = pageSize < PageSizeMin ? PageSizeMin : pageSize;
        pageSize = pageSize > PageSizeMax ? PageSizeMax : pageSize;

        return pageSize;
    }
}
