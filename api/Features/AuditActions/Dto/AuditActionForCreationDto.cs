using System;

namespace Features.AuditActions.Dto;

public sealed record AuditActionForCreationDto
{
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
}
