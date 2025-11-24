using System;

namespace Features.AuditActions.Dto;

public sealed record AuditActionDto
{
    public required Guid AuditActionId { get; set; }
    public required Guid AuditId { get; set; }
    public required string Description { get; set; }
    public required bool IsComplete { get; set; }
    public required long LastVersion { get; set; }
}
