namespace Core.Pagination;

public interface IPageableRequest
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}

public static class PageableRequest
{
    // Don't let the user pass page number -1 in the URL params
    private const int minPageNumber = 1;
    private const int minPageSize = 1;
    private const int maxPageSize = 20;
    private const int defaultPageSize = 5;

    public static int GetPageNumber(int? currentPageNumber = null)
    {
        int pageNumber = currentPageNumber ?? minPageNumber;
        pageNumber = pageNumber < minPageNumber ? minPageNumber : pageNumber;

        return pageNumber;
    }

    public static int GetPageSize(int? currentPageSize = null)
    {
        int pageSize = currentPageSize ?? defaultPageSize;
        pageSize = pageSize < minPageSize ? minPageSize : pageSize;
        pageSize = pageSize > maxPageSize ? maxPageSize : pageSize;

        return pageSize;
    }
}
