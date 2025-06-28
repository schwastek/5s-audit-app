using System;
using System.Collections.Generic;

namespace Domain.Events.ChangeTracking;

public class EntityChangeTracker
{
    private readonly Dictionary<string, MemberChanged> _changes = [];
    public IReadOnlyCollection<MemberChanged> Changes => _changes.Values;

    public void Add<T>(MemberChanged<T> newChange)
    {
        var key = newChange.MemberName;

        if (_changes.TryGetValue(key, out var previousChange))
        {
            switch (previousChange)
            {
                case CollectionChanged<T> previousCollectionChange when newChange is CollectionChanged<T> newCollectionChange:
                    previousCollectionChange.MergeWith(newCollectionChange);
                    if (previousCollectionChange.NoChanges()) _changes.Remove(key);
                    return;

                case PropertyChanged<T> previousPropertyChange when newChange is PropertyChanged<T> newPropertyChange:
                    previousPropertyChange.MergeWith(newPropertyChange);
                    if (previousPropertyChange.NoChanges()) _changes.Remove(key);
                    return;

                default:
                    throw new InvalidOperationException(
                        $"Change already exists for member '{key}' with a different type. " +
                        $"Existing type is '{previousChange.GetType().Name}', new type is '{newChange.GetType().Name}'.");
            }
        }
        else
        {
            // Add if not exists yet.
            if (!newChange.NoChanges())
            {
                _changes.Add(newChange.MemberName, newChange);
            }
        }
    }

    public bool HasChanges => _changes.Count > 0;
    public void Clear() => _changes.Clear();
}
