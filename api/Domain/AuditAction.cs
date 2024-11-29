using Domain.Exceptions;
using System;

namespace Domain;

public sealed class AuditAction
{
    public const int DescriptionMaxLength = 280;

    public Guid AuditActionId { get; private set; }
    public string Description { get; private set; }
    public bool IsComplete { get; private set; }

    public Guid AuditId { get; internal set; }

    // EF Core calls this constructor when creating an instance of the entity.
    private AuditAction(Guid auditActionId, string description, bool isComplete)
    {
        AuditActionId = auditActionId;
        Description = description;
        IsComplete = isComplete;
    }

    public static AuditAction Create(Guid auditActionId, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidEntityException(ErrorCodes.AuditAction.DescriptionIsRequired);
        }

        if (description.Length > DescriptionMaxLength)
        {
            throw new InvalidEntityException(ErrorCodes.AuditAction.DescriptionIsTooLong);
        }

        var auditAction = new AuditAction(auditActionId, description, isComplete: false);

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
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidEntityException(ErrorCodes.AuditAction.DescriptionIsRequired);
        }

        if (description.Length > DescriptionMaxLength)
        {
            throw new InvalidEntityException(ErrorCodes.AuditAction.DescriptionIsTooLong);
        }

        Description = description;
    }
}
