using Infrastructure.MappingService;

namespace Api.Requests.Common;

public sealed class PaginationMetadataMapper : IMapper<Infrastructure.Pagination.PaginationMetadata, Requests.Common.PaginationMetadata>
{
    public PaginationMetadata Map(Infrastructure.Pagination.PaginationMetadata src)
    {
        return new PaginationMetadata()
        {
            CurrentPage = src.CurrentPage,
            PageSize = src.PageSize,
            HasNextPage = src.HasNextPage,
            HasPreviousPage = src.HasPreviousPage,
            TotalCount = src.TotalCount,
            TotalPages = src.TotalPages
        };
    }
}
