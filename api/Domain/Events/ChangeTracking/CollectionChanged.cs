using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Events.ChangeTracking;

public abstract class CollectionChanged<T> : MemberChanged<T>
{
    private readonly HashSet<T> _addedItems;
    private readonly HashSet<T> _removedItems;

    public IReadOnlySet<T> AddedItems => _addedItems;
    public IReadOnlySet<T> RemovedItems => _removedItems;

    public CollectionChanged(string collectionName, IEnumerable<T>? added, IEnumerable<T>? removed, IEqualityComparer<T>? comparer)
        : base(collectionName, comparer)
    {
        _addedItems = added?.ToHashSet(comparer) ?? [];
        _removedItems = removed?.ToHashSet(comparer) ?? [];
    }

    public CollectionChanged<T> MergeWith(CollectionChanged<T> other)
    {
        if (!MemberName.Equals(other.MemberName))
        {
            throw new InvalidOperationException($"Merge failed: properties mismatch. This event tracks '{MemberName}', but attempted to merge with '{other.MemberName}'.");
        }

        _addedItems.UnionWith(other._addedItems);
        _removedItems.UnionWith(other._removedItems);

        // Snapshot current sets to ensure cancel-out logic isn't affected by prior mutations.
        var addedSnapshot = _addedItems.ToHashSet(Comparer);
        var removedSnapshot = _removedItems.ToHashSet(Comparer);

        // Cancel out opposing changes (added then removed = no change).
        _addedItems.ExceptWith(removedSnapshot);
        _removedItems.ExceptWith(addedSnapshot);

        return this;
    }

    public override bool NoChanges()
    {
        // SetEquals internally uses the comparer that was passed to the HashSet<T> constructor.
        return _addedItems.SetEquals(_removedItems);
    }

    public override string ToString()
    {
        return $"Updated {MemberName}: added [{_addedItems.Count}]; removed [{_removedItems.Count}].";
    }
}
