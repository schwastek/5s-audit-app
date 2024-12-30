using MediatR;
using System;

namespace Features.AuditAction.Update;

public sealed record UpdateAuditActionCommand : IRequest
{
    public required Guid AuditActionId { get; init; }
    public required string Description { get; init; }
    public required bool IsComplete { get; init; }
}
