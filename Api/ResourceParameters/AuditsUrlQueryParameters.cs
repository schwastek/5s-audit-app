namespace Api.ResourceParameters;

public class AuditsUrlQueryParameters
{
    // Don't let the user pass page number -1 in the URL params
    private const int minPageNumber = 1;
    private const int maxPageSize = 20;

    private int _pageNumber = 1;
    private int _pageSize = 1;

    /// <example>1</example>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value < minPageNumber) ? minPageNumber : value; 
    }

    /// <example>5</example>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    /// <example>author asc</example>
    public string OrderBy { get; set; } = "author asc";
}
