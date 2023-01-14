using Api.DbContexts;
using System;
using System.Linq;

namespace Api.Services;

public class AuditService
{
    private readonly LeanAuditorContext context;

    public AuditService(LeanAuditorContext context)
    {
        this.context = context;
    }

    public bool AuditExists(Guid auditId)
    {
        if (auditId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(auditId));
        }

        return context.Audits.Any(a => a.AuditId == auditId);
    }
}
