﻿using Api.Commands;
using Api.DbContexts;
using Api.Domain;
using Api.Exceptions;
using Api.Mappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Handlers
{
    public sealed class DeleteAuditActionHandler : IRequestHandler<DeleteAuditActionCommand, Unit>
    {
        private readonly LeanAuditorContext context;

        public DeleteAuditActionHandler(LeanAuditorContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteAuditActionCommand request, CancellationToken cancellationToken)
        {
            AuditAction auditAction = await context.AuditActions.FindAsync(request.ActionId);

            if (auditAction == null)
            {
                throw new ActionNotFoundException(request.ActionId);
            }

            context.Remove(auditAction);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}