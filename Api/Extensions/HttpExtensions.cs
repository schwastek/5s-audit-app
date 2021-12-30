using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Api.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
            int pageSize, int totalCount, int totalPages,
            string previousPageLink, string nextPageLink)
        {
            var paginationMetadata = new
            {
                currentPage = currentPage,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPages = totalPages,
                previousPage = previousPageLink,
                nextPage = nextPageLink
            };

            response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        }
    }
}
