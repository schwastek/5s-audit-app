using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Events;

public abstract class CollectionChangedEvent : ChangedEvent
{
    public const string MemberNamePrefix = "Collection";

    public CollectionChangedEvent(string collectionName) : base($"{MemberNamePrefix}.{collectionName}")
    {
    }
}

public abstract class CollectionChangedEvent<T> : CollectionChangedEvent
{
    public IReadOnlyList<T> AddedItems { get; }
    public IReadOnlyList<T> RemovedItems { get; }
    public IEqualityComparer<T> Comparer { get; }

    public abstract CollectionChangedEvent<T> Clone(IEnumerable<T>? addedItems = null, IEnumerable<T>? removedItems = null);

    public CollectionChangedEvent(string collectionName, IEnumerable<T>? addedItems, IEnumerable<T>? removedItems, IEqualityComparer<T>? comparer)
        : base(collectionName)
    {
        AddedItems = addedItems?.ToList() ?? [];
        RemovedItems = removedItems?.ToList() ?? [];
        Comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public virtual CollectionChangedEvent<T> MergeWith(CollectionChangedEvent<T> other)
    {
        if (!MemberName.Equals(other.MemberName))
        {
            throw new InvalidOperationException("Cannot merge events with different collection names.");
        }

        var allAdded = AddedItems.Concat(other.AddedItems).ToList();
        var allRemoved = RemovedItems.Concat(other.RemovedItems).ToList();

        var addedSet = allAdded.ToHashSet(Comparer);
        var removedSet = allRemoved.ToHashSet(Comparer);

        // Cancel out opposing changes (added then removed = no change).
        addedSet.ExceptWith(allRemoved);
        removedSet.ExceptWith(allAdded);

        return Clone(addedSet, removedSet);
    }

    public virtual bool IsEmpty()
    {
        return AddedItems.Count == 0 && RemovedItems.Count == 0;
    }

    public override string GetDescription()
    {
        return $"Updated {MemberName}: added [{AddedItems.Count}]; removed [{AddedItems.Count}].";
    }
}
