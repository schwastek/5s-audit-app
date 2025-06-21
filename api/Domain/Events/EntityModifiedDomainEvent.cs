using System.Collections.Generic;
using System.Linq;

namespace Domain.Events;

public abstract class EntityModifiedDomainEvent : DomainEvent
{
    private readonly Dictionary<string, ChangedEvent> _changedEvents = [];

    internal ChangedEvent CurrentChangedEvent { get; }
    public IReadOnlyList<ChangedEvent> ChangedEvents => _changedEvents.Values.ToList().AsReadOnly();

    public EntityModifiedDomainEvent(ChangedEvent changedEvent)
    {
        CurrentChangedEvent = changedEvent;
    }

    public virtual EntityModifiedDomainEvent MergeWith(EntityModifiedDomainEvent other)
    {
        var memberName = other.CurrentChangedEvent.MemberName;

        // Replace if already exists or add.
        _changedEvents[memberName] = other.CurrentChangedEvent;

        return this;
    }
}
