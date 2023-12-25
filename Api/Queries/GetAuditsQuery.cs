using Api.Common;
using Api.Models;
using MediatR;
using System.Collections.Generic;

namespace Api.Queries;

public sealed record GetAuditsQuery :
    IRequest<GetAuditsQueryResult>,
    IPageableQuery,
    IOrderByQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string OrderBy { get; init; } = string.Empty;
}

public class GetAuditsQueryResult : IPaginatedResult<AuditListDto>
{
    // TODO: Change `null!` to `required` when C# 11
    public IReadOnlyList<AuditListDto> Items { get; init; } = null!;
    public IPaginationMetadata Metadata { get; init; } = null!;
}
