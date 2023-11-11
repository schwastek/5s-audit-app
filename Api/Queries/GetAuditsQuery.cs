using Api.Common;
using Api.Models;
using MediatR;
using System.Collections.Generic;

namespace Api.Queries;

public sealed record GetAuditsQuery() : IRequest<GetAuditsQueryResult>, IPageableQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string OrderBy { get; init; } = string.Empty;
}

public class GetAuditsQueryResult : PagedResult<AuditListDto>
{
    public GetAuditsQueryResult(List<AuditListDto> items, IPageableQuery query) : base(items, query)
    {
    }
}
