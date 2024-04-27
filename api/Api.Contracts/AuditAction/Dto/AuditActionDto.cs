using System;

namespace Api.Contracts.AuditAction.Dto
{
    public class AuditActionDto
    {
        /// <example>ac1a0251-46cf-452b-9911-cfc998ea41a9</example>
        public Guid ActionId { get; set; }

        /// <example>f4940d26-7c0a-4ab6-b1cd-da8f708c5819</example>
        public Guid AuditId { get; set; }

        /// <example>Clean up</example>
        public string Description { get; set; }

        /// <example>false</example>
        public bool IsComplete { get; set; }
    }
}
