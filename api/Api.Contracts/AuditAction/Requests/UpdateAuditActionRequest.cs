using System;

namespace Api.Contracts.AuditAction.Requests
{
    public class UpdateAuditActionRequest
    {
        /// <example>2677f242-05ee-4de8-8f42-9dc2ada8d368</example>
        public Guid? ActionId { get; set; }

        /// <example>Clean up (UPDATED)</example>
        public string? Description { get; set; }

        /// <example>true</example>
        public bool? IsComplete { get; set; }
    }
}
