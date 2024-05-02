using Api.Contracts.Audit.Dto;
using Api.Contracts.Common.Requests;
using System.Collections.Generic;

namespace Api.Contracts.Audit.Requests
{
    public class ListAuditsRequest : IPageableRequest, IOrderByRequest
    {
        /// <inheritdoc/>
        public string OrderBy { get; set; } = null!;

        /// <inheritdoc/>
        public int? PageNumber { get; set; }

        /// <inheritdoc/>
        public int? PageSize { get; set; }
    }

    public class ListAuditsResponse : IPaginatedResult<AuditListItemDto>
    {
        public IReadOnlyCollection<AuditListItemDto> Items { get; set; } = null!;
        public PaginationMetadata Metadata { get; set; } = null!;
    }
}
