using Api.Common;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Api.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader<T>(this HttpResponse response, PagedResult<T> result, string? previousPageLink, string? nextPageLink)
    {
        var paginationMetadata = new
        {
            currentPage = result.CurrentPage,
            pageSize = result.PageSize,
            totalCount = result.TotalCount,
            totalPages = result.TotalPages,
            previousPage = previousPageLink,
            nextPage = nextPageLink
        };

        response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    }
}
