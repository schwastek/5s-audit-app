using MediatR;
using System;

namespace Features.AuditActions.Delete;

public sealed record DeleteAuditActionCommand : IRequest
{
    public Guid AuditActionId { get; set; }
}
