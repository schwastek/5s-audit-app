using Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Features.Audits.BusinessRules;

public interface IAuditBusinessRules
{
    public Task<bool> AuditExists(Guid auditId, CancellationToken cancellationToken);
}

public class AuditBusinessRules : IAuditBusinessRules
{
    private readonly LeanAuditorContext _context;

    public AuditBusinessRules(LeanAuditorContext context)
    {
        _context = context;
    }

    public async Task<bool> AuditExists(Guid auditId, CancellationToken cancellationToken)
    {
        if (auditId == Guid.Empty)
        {
            return false;
        }

        var result = await _context.Audits.AnyAsync(a => a.AuditId == auditId, cancellationToken);

        return result;
    }
}
