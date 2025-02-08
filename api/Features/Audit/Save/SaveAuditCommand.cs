using MediatR;
using System;
using System.Collections.Generic;

namespace Features.Audit.Save;

public sealed record SaveAuditCommand : IRequest<SaveAuditCommandResult>
{
    public required Guid AuditId { get; init; }
    public required string Author { get; init; }
    public required string Area { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    public required ICollection<Answer.Dto.AnswerForCreationDto> Answers { get; init; }
    public required ICollection<AuditAction.Dto.AuditActionForCreationDto> Actions { get; init; }
}

public sealed record SaveAuditCommandResult
{
    public Guid AuditId { get; init; }
}
