using Api.Models;
using MediatR;

namespace Api.Queries;

public sealed record CreateAuditActionCommand(AuditActionForCreationDto AuditAction) 
    : IRequest<AuditActionDto>;
