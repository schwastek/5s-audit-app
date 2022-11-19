using Api.Models;
using MediatR;
using System;

namespace Api.Queries
{
    public sealed record GetAuditQuery(Guid AuditId) : IRequest<AuditDto>;
}
