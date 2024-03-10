using Core.MappingService;

namespace Core.Pagination;

public class PaginationMapper :
    IMapper<PaginationMetadata, Api.Contracts.Common.Requests.PaginationMetadata>
{
    public Api.Contracts.Common.Requests.PaginationMetadata Map(PaginationMetadata src)
    {
        return new Api.Contracts.Common.Requests.PaginationMetadata()
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
