using MediatR;
using System;

namespace Features.AuditAction.Save;

public sealed record SaveAuditActionCommand : IRequest<SaveAuditActionCommandResult>
{
    public required Guid AuditId { get; init; }
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
}

public sealed record SaveAuditActionCommandResult
{
    public required Guid AuditActionId { get; set; }
    public required Guid AuditId { get; set; }
    public required string Description { get; set; }
    public required bool IsComplete { get; set; }
}
