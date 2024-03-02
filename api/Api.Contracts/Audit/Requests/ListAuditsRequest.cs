using Api.Contracts.Audit.Dto;
using Api.Contracts.Common.Requests;
using System.Collections.Generic;

namespace Api.Contracts.Audit.Requests
{
    public class ListAuditsRequest : PageableRequest
    {
        /// <example>author asc</example>
        public string? OrderBy { get; set; }
    }

    public class ListAuditsResponse : IPaginatedResult<AuditListItemDto>
    {
        public IReadOnlyCollection<AuditListItemDto> Items { get; set; } = null!;
        public IPaginationMetadata Metadata { get; set; } = null!;
    }
}
