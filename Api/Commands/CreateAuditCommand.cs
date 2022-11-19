using Api.Domain;
using Api.Models;
using MediatR;

namespace Api.Queries
{
    public sealed record CreateAuditCommand(AuditForCreationDto Audit) : IRequest<AuditDto>;
}
