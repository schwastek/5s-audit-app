namespace Domain.Events;

/// <summary>
/// Marker class to represent a Changed Event.
/// </summary>
public abstract class ChangedEvent
{
    public string MemberName { get; }
    public abstract EntityModifiedDomainEvent AsDomainEvent();

    protected ChangedEvent(string memberName)
    {
        MemberName = memberName;
    }

    public virtual string GetDescription()
    {
        return $"Member '{MemberName}' changed.";
    }

    public override string ToString() => GetDescription();
}
