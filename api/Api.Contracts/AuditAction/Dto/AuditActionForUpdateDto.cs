﻿namespace Api.Contracts.AuditAction.Dto
{
    public class AuditActionForUpdateDto
    {
        /// <example>Clean up (UPDATED)</example>
        public string? Description { get; set; }

        /// <example>true</example>
        public bool? IsComplete { get; set; }
    }
}
