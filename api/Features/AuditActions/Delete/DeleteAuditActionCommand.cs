using System;

namespace Features.AuditActions.Delete;

public sealed record DeleteAuditActionCommand
{
    public Guid AuditActionId { get; set; }
}
