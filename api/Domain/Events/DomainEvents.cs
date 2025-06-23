using System.Collections.Generic;

namespace Domain.Events;

public abstract class DomainEvents
{
    protected List<DomainEvent> Events { get; } = [];

    public virtual void Add(DomainEvent eventItem)
    {
        Events.Add(eventItem);
    }

    public virtual void Add(EntityChangedEvent eventItem)
    {
        // There should be only one event of that type.
        // Check for an existing event. If found, merge.
        var index = Events.FindIndex(x => x is EntityChangedEvent);

        if (index >= 0)
        {
            var previous = Events[index] as EntityChangedEvent;
            // Event self-updates. Just reposition it as the most recent.
            Events.RemoveAt(index);
            Events.Add(eventItem);
        }
        else
        {
            Events.Add(eventItem);
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
