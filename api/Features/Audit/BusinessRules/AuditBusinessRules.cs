using Data.DbContext;
using System;
using System.Linq;

namespace Features.Audit.BusinessRules;

public interface IAuditBusinessRules
{
    public bool AuditExists(Guid auditId);
}

public class AuditBusinessRules : IAuditBusinessRules
{
    private readonly LeanAuditorContext context;

    public AuditBusinessRules(LeanAuditorContext context)
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
