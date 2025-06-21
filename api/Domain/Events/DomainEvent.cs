using System.Collections.Generic;
using System.Linq;

namespace Domain.Events;

public interface IHaveDomainEvents
{
    IReadOnlyList<DomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}

/// <summary>
/// Marker class to represent a Domain Event.
/// </summary>
public abstract class DomainEvent { }

public abstract class DomainEvents
{
    protected List<DomainEvent> Events { get; } = [];
    protected List<ChangedEvent> Changes { get; } = [];

    public virtual void Add(DomainEvent eventItem)
    {
        Events.Add(eventItem);
    }

    public virtual void Add<T>(CollectionChangedEvent<T> change)
    {
        // There should be only one event per entity property.
        // Check for an existing event for the same class member. If found, merge.
        if (change.IsEmpty()) return;

        var previous = Changes
            .OfType<CollectionChangedEvent<T>>()
            .Where(e => e.MemberName.Equals(change.MemberName))
            .SingleOrDefault();

        if (previous is null)
        {
            Changes.Add(change);
            Add(change.AsDomainEvent());
            return;
        }

        // Replace ChangedEvent.
        Changes.Remove(previous);
        var merged = previous.MergeWith(change);
        if (merged.IsEmpty()) return;
        Changes.Add(merged);

        // Add DomainEvent.
        Add(merged.AsDomainEvent());
    }

    public virtual void Add(EntityModifiedDomainEvent eventItem)
    {
        // There should be only one event of that type.
        // Check for an existing event. If found, merge.
        EntityModifiedDomainEvent? previous = null;

        var index = Events.FindIndex(x => x is EntityModifiedDomainEvent);
        if (index >= 0) previous = Events[index] as EntityModifiedDomainEvent;

        if (previous is null)
        {
            Events.Add(eventItem);
        }
        else
        {
            // Update the event and place it as the most recent.
            var updated = previous.MergeWith(eventItem);
            Events.RemoveAt(index);
            Events.Add(updated);
        }
    }

    public virtual IReadOnlyList<DomainEvent> Get()
    {
        return Events.AsReadOnly();
    }

    public virtual void Clear()
    {
        Events.Clear();
    }
}
