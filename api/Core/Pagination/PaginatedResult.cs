using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Pagination;

public interface IPaginatedResultFactory<T>
{
    Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, IPageableQuery query, CancellationToken cancellationToken);
}

public class PaginatedResultFactory<T> : IPaginatedResultFactory<T>
{
    public async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, IPageableQuery query, CancellationToken cancellationToken)
    {
        int count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var metadata = new PaginationMetadata(count, query);
        var result = new PaginatedResult<T>(items, metadata);

        return result;
    }
}

public interface IPaginatedResult<T>
{
    IReadOnlyList<T> Items { get; }
    PaginationMetadata Metadata { get; }
}

public class PaginatedResult<T> : IPaginatedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public PaginationMetadata Metadata { get; }

    public PaginatedResult(List<T> items, PaginationMetadata metadata)
    {
        Items = items.AsReadOnly();
        Metadata = metadata;
    }
}

public class PaginationMetadata
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
