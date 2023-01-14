using Api.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Api.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, MetaData metaData, string? previousPageLink, string? nextPageLink)
    {
        var paginationMetadata = new
        {
            currentPage = metaData.CurrentPage,
            pageSize = metaData.PageSize,
            totalCount = metaData.TotalCount,
            totalPages = metaData.TotalPages,
            previousPage = previousPageLink,
            nextPage = nextPageLink
        };

        response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    }
}
