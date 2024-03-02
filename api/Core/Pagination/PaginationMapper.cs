using Core.MappingService;

namespace Core.Pagination;

public class PaginationMapper :
    IMapper<Core.Pagination.IPaginationMetadata, Api.Contracts.Common.Requests.IPaginationMetadata>
{
    public Api.Contracts.Common.Requests.IPaginationMetadata Map(IPaginationMetadata src)
    {
        return new Api.Contracts.Common.Requests.PaginationMetadata()
        {
            CurrentPage = src.CurrentPage,
            PageSize = src.PageSize,
            HasNextPage = src.HasNextPage,
            HasPreviousPage = src.HasPreviousPage,
            TotalCount = src.TotalCount,
            TotalPages = src.TotalPages,
        };
    }
}
