namespace Features.AuditAction.Dto;

public sealed record AuditActionForUpdateDto
{
    public required string Description { get; init; }
    public required bool IsComplete { get; set; }
}
