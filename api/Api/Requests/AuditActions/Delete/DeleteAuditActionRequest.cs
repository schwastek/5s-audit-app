using System;

namespace Api.Requests.AuditActions.Delete;

public sealed record DeleteAuditActionRequest
{
    /// <example>12aaf1bd-9aae-470d-8989-b991d8df8298</example>
    public Guid AuditActionId { get; set; }
}
