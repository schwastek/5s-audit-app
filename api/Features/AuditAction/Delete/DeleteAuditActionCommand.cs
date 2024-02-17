using MediatR;
using System;

namespace Features.AuditAction.Delete;

public sealed record DeleteAuditActionCommand : IRequest
{
    public Guid ActionId { get; set; }
}
