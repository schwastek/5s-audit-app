using System;

namespace Features.AuditActions.Update;

public sealed record UpdateAuditActionCommand
{
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
    public required bool IsComplete { get; init; }
    public required long LastVersion { get; init; }
}

public sealed record UpdateAuditActionCommandResult
{
    public required Guid AuditActionId { get; set; }
    public required string Description { get; set; }
    public required bool IsComplete { get; set; }
    public required long LastVersion { get; set; }
}
