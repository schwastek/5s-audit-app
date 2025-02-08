using System;

namespace Features.AuditAction.Dto;

public sealed record AuditActionForCreationDto
{
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
}
