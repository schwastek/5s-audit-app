namespace Api.Contracts.AuditAction.Requests
{
    public class UpdateAuditActionRequest
    {
        /// <example>Clean up (UPDATED)</example>
        public string Description { get; set; } = string.Empty;

        /// <example>false</example>
        public bool IsComplete { get; set; }
    }
}
