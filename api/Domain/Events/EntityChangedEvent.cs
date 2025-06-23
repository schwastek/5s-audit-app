using System;
using System.Collections.Generic;

namespace Domain.Events;

public abstract class EntityChangedEvent : DomainEvent
{
    private readonly List<MemberChangedEvent> _changes = [];
    public IReadOnlyList<MemberChangedEvent> Changes => _changes.AsReadOnly();

    public virtual void Add<T>(CollectionChangedEvent<T> newChange)
    {
        if (newChange.NoChange()) return;

        // Check for an existing event for the same class member. If found, merge.
        // There should be only one event per entity's property.
        var index = _changes.FindIndex(x => x.MemberName.Equals(newChange.MemberName));

        if (index >= 0)
        {
            var previous = _changes[index];

            // Ensure the type of the existing item matches the new item's type.
            if (previous is CollectionChangedEvent<T> previousChange)
            {
                // Update the item and reposition it at the end to indicate latest update.
                previousChange.MergeWith(newChange);
                _changes.RemoveAt(index);
                _changes.Add(previousChange);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Change with '{newChange.MemberName}' already exists with a different type. " +
                    $"Existing type is '{previous.GetType().Name}', new type is '{newChange.GetType().Name}'");
            }
        }
        else
        {
            // Add if not exists yet.
            _changes.Add(newChange);
        }
    }
}
