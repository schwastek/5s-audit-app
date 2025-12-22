using Features.Audits.Dto;
using Infrastructure.OrderBy;
using Infrastructure.Pagination;
using System.Collections.Generic;

namespace Features.Audits.List;

public sealed record ListAuditsQuery : IPageableQuery, IOrderByQuery
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required IReadOnlyList<OrderByInstruction> OrderBy { get; init; }
}

public class ListAuditsQueryResult : IPaginatedResult<AuditListItemDto>
{
    public required IReadOnlyList<AuditListItemDto> Items { get; init; }
    public required PaginationMetadata Metadata { get; init; }
}
