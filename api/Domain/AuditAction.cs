using Domain.Auditing;
using System;

namespace Domain;

public sealed class AuditAction : IAuditableEntity
{
    public Guid AuditActionId { get; private set; }
    public string Description { get; private set; }
    public bool IsComplete { get; private set; }

    public Guid AuditId { get; internal set; }

    public string CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public string ModifiedBy { get; private set; }
    public DateTimeOffset ModifiedAt { get; private set; }

    // EF Core calls this constructor when creating an instance of the entity.
    private AuditAction(Guid auditActionId, string description, bool isComplete,
        string createdBy, DateTimeOffset createdAt, string modifiedBy, DateTimeOffset modifiedAt)
    {
        AuditActionId = auditActionId;
        Description = description;
        IsComplete = isComplete;

        // Set auditable properties.
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
        ModifiedBy = modifiedBy;
    }

    public static AuditAction Create(Guid auditActionId, string description)
    {
        // Set auditable properties.
        var createdBy = AuditPlaceholders.Unknown;
        var createdAt = DateTimeOffset.UtcNow;

        var auditAction = new AuditAction(
            auditActionId: auditActionId,
            description: description,
            isComplete: false,
            createdBy: createdBy,
            createdAt: createdAt,
            modifiedBy: createdBy,
            modifiedAt: createdAt);

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
