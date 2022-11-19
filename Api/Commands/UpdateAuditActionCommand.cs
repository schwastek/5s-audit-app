using Api.Models;
using MediatR;
using System;

namespace Api.Commands
{
    public sealed record UpdateAuditActionCommand(Guid ActionId, AuditActionForUpdateDto AuditAction) 
        : IRequest;
}
