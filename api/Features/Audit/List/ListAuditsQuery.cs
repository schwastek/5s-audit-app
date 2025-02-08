using Core.OrderByService;
using Core.Pagination;
using Features.Audit.Dto;
using MediatR;
using System.Collections.Generic;

namespace Features.Audit.List;

public sealed record ListAuditsQuery : IRequest<ListAuditsQueryResult>, IPageableQuery, IOrderByQuery
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
