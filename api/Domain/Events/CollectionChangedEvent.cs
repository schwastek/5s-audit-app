using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Events;

public interface ICollectionChangedEvent { }

public abstract class CollectionChangedEvent<T> : MemberChangedEvent<T>, ICollectionChangedEvent
{
    private readonly HashSet<T> _addedItems;
    private readonly HashSet<T> _removedItems;

    public IReadOnlySet<T> AddedItems => _addedItems;
    public IReadOnlySet<T> RemovedItems => _removedItems;

    public CollectionChangedEvent(string collectionName, IEnumerable<T>? added, IEnumerable<T>? removed, IEqualityComparer<T>? comparer)
        : base(collectionName, comparer)
    {
        _addedItems = added?.ToHashSet(comparer) ?? [];
        _removedItems = removed?.ToHashSet(comparer) ?? [];
    }

    public virtual CollectionChangedEvent<T> MergeWith(CollectionChangedEvent<T> other)
    {
        if (!MemberName.Equals(other.MemberName))
        {
            throw new InvalidOperationException("Cannot merge events with different collection names.");
        }

        _addedItems.UnionWith(other._addedItems);
        _removedItems.UnionWith(other._removedItems);

        // Cancel out opposing changes (added then removed = no change).
        _addedItems.ExceptWith(_removedItems);
        _removedItems.ExceptWith(_addedItems);

        return this;
    }

    public virtual bool NoChange()
    {
        return AddedItems.Count == 0 && RemovedItems.Count == 0;
    }

    public override string GetDescription()
    {
        return $"Updated {MemberName}: added [{AddedItems.Count}]; removed [{AddedItems.Count}].";
    }
}
