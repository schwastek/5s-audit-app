using Api.Contracts.Audit.Dto;
using Api.Contracts.Common;
using System;

namespace Api.Contracts.Audit.Requests
{
    public class GetAuditRequest
    {
        /// <example>12aaf1bd-9aae-470d-8989-b991d8df8298</example>
        public Guid Id { get; set; }
    }

    public class GetAuditResponse
    {
        [SwaggerRequired]
        public AuditDto Audit { get; set; }
    }
}
