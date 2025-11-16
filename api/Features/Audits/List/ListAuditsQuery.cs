using Features.Audits.Dto;
using Infrastructure.OrderByService;
using Infrastructure.Pagination;
using System.Collections.Generic;

namespace Features.Audits.List;

public sealed record ListAuditsQuery : IPageableQuery, IOrderByQuery
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required string OrderBy { get; init; }
}

public class ListAuditsQueryResult : IPaginatedResult<AuditListItemDto>
{
    public required IReadOnlyList<AuditListItemDto> Items { get; init; }
    public required PaginationMetadata Metadata { get; init; }
}
