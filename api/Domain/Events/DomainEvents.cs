using Domain.Events.ChangeTracking;
using System.Collections.Generic;

namespace Domain.Events;

public class DomainEvents
{
    private readonly List<DomainEvent> _events = [];

    public void Add(DomainEvent eventItem)
    {
        _events.Add(eventItem);
    }

    public void Add(EntityChangedEvent eventItem)
    {
        // There should be only one event of that type.
        // Check for an existing event.
        var index = _events.FindIndex(e => e is EntityChangedEvent);

        if (index == -1)
        {
            _events.Add(eventItem);
        }
    }

    public virtual IReadOnlyList<DomainEvent> Collect()
    {
        return _events;
    }

    public void Clear()
    {
        _events.Clear();
    }
}
