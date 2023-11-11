using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Common;

public abstract class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPrevious { get; }
    public bool HasNext { get; }

    public PagedResult(List<T> items, IPageableQuery query)
    {
        Items = items.AsReadOnly();
        TotalCount = items.Count();
        PageSize = query.PageSize;
        CurrentPage = query.PageNumber;
        // If you have 51 reocrds in total, you get 6 pages in total,
        // reserving an extra page for the remaining data.
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        HasPrevious = (CurrentPage > 1);
        HasNext = (CurrentPage < TotalPages);
    }
}
