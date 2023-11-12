using System;
using System.Collections.Generic;

namespace Api.Common;

public abstract class PaginatedResult<T>
{
    // TODO: Change `null!` to `required` when C# 11 is available
    public IReadOnlyList<T> Items { get; init; } = null!;
    public IPaginationMetadata Metadata { get; init; } = null!;
}

public interface IPaginationMetadata
{
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}

public class PaginationMetadata : IPaginationMetadata
{
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }

    public PaginationMetadata(int count, IPageableQuery query)
    {
        TotalCount = count;
        CurrentPage = query.PageNumber;
        PageSize = query.PageSize;

        // If you have 51 reocrds in total, you get 6 pages in total,
        // reserving an extra page for the remaining data.
        TotalPages = (int)Math.Ceiling(count / (double)PageSize);

        HasPreviousPage = CurrentPage > 1;
        HasNextPage = CurrentPage < TotalPages;
    }
}
