using System;

namespace Features.AuditActions.Update;

public sealed record UpdateAuditActionCommand
{
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
    public required bool IsComplete { get; init; }
}
