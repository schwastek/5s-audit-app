using Api.Commands;
using Api.DbContexts;
using Api.Domain;
using Api.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers
{
    public sealed class UpdateAuditActionHandler : IRequestHandler<UpdateAuditActionCommand, Unit>
    {
        private readonly LeanAuditorContext context;

        public UpdateAuditActionHandler(LeanAuditorContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateAuditActionCommand request, CancellationToken cancellationToken)
        {
            AuditAction auditAction = await context.AuditActions.FindAsync(request.ActionId);

            if (auditAction == null)
            {
                throw new ActionNotFoundException(request.ActionId);
            }

            auditAction.Description = request.AuditAction.Description ?? auditAction.Description;
            auditAction.IsComplete = request.AuditAction.IsComplete;

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
