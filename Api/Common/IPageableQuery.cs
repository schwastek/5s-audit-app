namespace Api.Common;

public interface IPageableQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}