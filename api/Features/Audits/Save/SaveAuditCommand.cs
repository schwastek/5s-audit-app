using Features.Answers.Dto;
using Features.AuditActions.Dto;
using System;
using System.Collections.Generic;

namespace Features.Audits.Save;

public sealed record SaveAuditCommand
{
    public required Guid AuditId { get; init; }
    public required string Author { get; init; }
    public required string Area { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required ICollection<AnswerForCreationDto> Answers { get; init; }
    public required ICollection<AuditActionForCreationDto> Actions { get; init; }
}

public sealed record SaveAuditCommandResult
{
    public Guid AuditId { get; init; }
}
