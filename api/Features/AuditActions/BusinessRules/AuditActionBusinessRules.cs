﻿using Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AuditActions.BusinessRules;

public interface IAuditActionBusinessRules
{
    public int DescriptionMaxLength { get; }
    public Task<bool> AuditActionExists(Guid auditActionId, CancellationToken cancellationToken);
}

public class AuditActionBusinessRules : IAuditActionBusinessRules
{
    private readonly LeanAuditorContext _context;

    public int DescriptionMaxLength => 280;

    public AuditActionBusinessRules(LeanAuditorContext context)
    {
        _context = context;
    }

    public async Task<bool> AuditActionExists(Guid auditActionId, CancellationToken cancellationToken)
    {
        if (auditActionId == Guid.Empty)
        {
            return false;
        }

        var result = await _context.AuditActions.AnyAsync(x => x.AuditActionId == auditActionId, cancellationToken);

        return result;
    }
}
