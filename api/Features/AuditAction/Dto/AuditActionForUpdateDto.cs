namespace Features.AuditAction.Dto;

public sealed record AuditActionForUpdateDto
{
    public string Description { get; init; } = null!;
    public bool IsComplete { get; set; }
}
