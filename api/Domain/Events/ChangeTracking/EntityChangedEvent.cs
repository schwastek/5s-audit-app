using System.Collections.Generic;
using System.Text;

namespace Domain.Events.ChangeTracking;

public abstract class EntityChangedEvent : DomainEvent
{
    public IReadOnlyCollection<MemberChanged> Changes { get; }

    public EntityChangedEvent(IReadOnlyCollection<MemberChanged> changes)
    {
        Changes = changes;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(Changes.Count);

        foreach (var change in Changes)
        {
            sb.Append(change);
        }

        return sb.ToString();
    }
}
