using Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditAction.BusinessRules;

public interface IAuditActionBusinessRules
{
    public Task<bool> AuditActionExists(Guid actionId, CancellationToken cancellationToken);
}

public class AuditActionBusinessRules : IAuditActionBusinessRules
{
    private readonly LeanAuditorContext context;

    public AuditActionBusinessRules(LeanAuditorContext context)
    {
        this.context = context;
    }

    public async Task<bool> AuditActionExists(Guid actionId, CancellationToken cancellationToken)
    {
        if (actionId == Guid.Empty)
        {
            return false;
        }

        var result = await context.AuditActions.AnyAsync(x => x.AuditActionId == actionId, cancellationToken);

        return result;
    }
}
