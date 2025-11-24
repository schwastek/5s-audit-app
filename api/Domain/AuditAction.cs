using Domain.Auditing;
using Domain.Concurrency;
using System;

namespace Domain;

public sealed class AuditAction : IAuditableEntity, IConcurrencyToken
{
    public Guid AuditActionId { get; private set; }
    public string Description { get; private set; }
    public bool IsComplete { get; private set; }

    public Guid AuditId { get; internal set; }

    public string CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string ModifiedBy { get; private set; }
    public DateTimeOffset ModifiedAt { get; private set; }

    public long Version { get; private set; }

    // EF Core calls this constructor when creating an instance of the entity.
    private AuditAction(Guid auditActionId, string description, bool isComplete,
        string createdBy, DateTimeOffset createdAt, string modifiedBy, DateTimeOffset modifiedAt, 
        long version)
    {
        AuditActionId = auditActionId;
        Description = description;
        IsComplete = isComplete;

        // Set auditable properties.
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
        ModifiedBy = modifiedBy;

        // Set initial version.
        Version = version;
    }

    public static AuditAction Create(Guid auditActionId, string description)
    {
        // Set auditable properties.
        var createdBy = AuditPlaceholders.Unknown;
        var createdAt = DateTimeOffset.UtcNow;

        // Domain sets the initial version. EF increments it for concurrency control.
        var initialVersion = 0;

        var auditAction = new AuditAction(
            auditActionId: auditActionId,
            description: description,
            isComplete: false,
            createdBy: createdBy,
            createdAt: createdAt,
            modifiedBy: createdBy,
            modifiedAt: createdAt,
            version: initialVersion);

        return auditAction;
    }

    public void MarkComplete()
    {
        IsComplete = true;
    }

    public void MarkIncomplete()
    {
        IsComplete = false;
    }

    public void SetCompletionStatus(bool isComplete)
    {
        if (isComplete)
        {
            MarkComplete();
        }
        else
        {
            MarkIncomplete();
        }
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }
}
