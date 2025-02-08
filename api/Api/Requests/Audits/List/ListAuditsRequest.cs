using Api.Requests.Audits.Dto;
using Api.Requests.Common;
using System.Collections.Generic;

namespace Api.Requests.Audits.List;

public sealed record ListAuditsRequest : IPageableRequest, IOrderByRequest
{
    /// <inheritdoc/>
    public string? OrderBy { get; set; }

    /// <inheritdoc/>
    public int? PageNumber { get; set; }

    /// <inheritdoc/>
    public int? PageSize { get; set; }
}

public sealed record ListAuditsResponse : IPaginatedResult<AuditListItemDto>
{
    public IReadOnlyCollection<AuditListItemDto> Items { get; set; } = [];
    public PaginationMetadata Metadata { get; set; } = new();
}
