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

    /// <example>5</example>
    public long LastVersion { get; set; }
}

public sealed record UpdateAuditActionResponse
{
    /// <example>ac1a0251-46cf-452b-9911-cfc998ea41a9</example>
    public Guid AuditActionId { get; set; }

    /// <example>Clean up</example>
    public string Description { get; set; } = string.Empty;

    /// <example>false</example>
    public bool IsComplete { get; set; }

    /// <example>5</example>
    public long LastVersion { get; set; }
}
