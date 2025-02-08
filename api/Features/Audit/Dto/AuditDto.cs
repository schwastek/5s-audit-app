using System;
using System.Collections.Generic;

namespace Features.Audit.Dto;

public sealed record AuditDto
{
    public required Guid AuditId { get; init; }
    public required string Author { get; init; }
    public required string Area { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required double Score { get; init; }
    public required IReadOnlyList<Answer.Dto.AnswerDto> Answers { get; init; }
    public required IReadOnlyList<AuditAction.Dto.AuditActionDto> Actions { get; init; }
}
