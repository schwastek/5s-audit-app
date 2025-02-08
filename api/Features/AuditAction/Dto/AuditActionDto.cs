using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionDto
{
    public required Guid AuditActionId { get; set; }
    public required Guid AuditId { get; set; }
    public required string Description { get; set; }
    public required bool IsComplete { get; set; }
}
