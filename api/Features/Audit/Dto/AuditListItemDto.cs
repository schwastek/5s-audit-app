using System;

namespace Features.Audit.Dto;

public class AuditListItemDto
{
    public required Guid AuditId { get; set; }
    public required string Author { get; set; }
    public required string Area { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required double Score { get; set; }
}
