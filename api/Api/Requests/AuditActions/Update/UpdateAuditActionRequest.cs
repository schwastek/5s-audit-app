using System;
using System.Text.Json.Serialization;

namespace Api.Requests.AuditActions.Update;

public sealed record UpdateAuditActionRequest
{
    /// <example>12aaf1bd-9aae-470d-8989-b991d8df8298</example>
    [JsonIgnore]
    public Guid AuditActionId { get; set; }

    /// <example>Clean up (UPDATED)</example>
    public string Description { get; set; } = string.Empty;

    /// <example>false</example>
    public bool IsComplete { get; set; }
}
