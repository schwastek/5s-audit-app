using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Events;

/// <summary>
/// Stores and manages domain events.
/// </summary>
public class DomainEvents
{
    // Stores all domain events in order.
    // LinkedList allows fast insertion, removal, and reordering.
    // If we used a List, we would need to track the index of each event per type.
    // On removal, all subsequent indices would shift and tracked indices would need to be updated.
    private readonly LinkedList<DomainEvent> _events = [];

    // Maps each event type to the set of nodes containing events of that type.
    // Enables O(1) access to all events of a specific type for efficient add, replace, or removal.
    private readonly Dictionary<Type, List<LinkedListNode<DomainEvent>>> _eventsByType = [];

    /// <summary>
    /// Adds a domain event to the list, even if another event of the same type already exists (duplicates allowed).
    /// Use when all occurrences of the event matter and should be captured independently.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserLoggedInEvent</c> — users can log in multiple times, each login is significant.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The event instance to add.</param>
    public void Add<T>(T domainEvent) where T : DomainEvent
    {
        var node = _events.AddLast(domainEvent);
        AddNode(typeof(T), node);
    }

    /// <summary>
    /// Adds a domain event only if no event of the same type has already been added.
    /// Use this when an event represents a unique change that should not repeat.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserActivatedEvent</c> — a user can only be activated once.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The event instance to add if not already present by type.</param>
    public void AddOnce<T>(T domainEvent) where T : DomainEvent
    {
        var type = typeof(T);
        if (!_eventsByType.ContainsKey(type))
        {
            Add(domainEvent);
        }
    }

    /// <summary>
    /// Removes the previous event of the same type (if present) and appends the new one to the end.
    /// Use when the latest version should overwrite any earlier one.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserLastSeenUpdatedEvent</c> — only the most recent login is relevant, earlier ones are obsolete.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The new event to add or replace.</param>
    public void AddOrReplaceLast<T>(T domainEvent) where T : DomainEvent
    {
        var type = typeof(T);

        if (_eventsByType.TryGetValue(type, out var nodes) && nodes.Count > 0)
        {
            var last = nodes[^1];
            _events.Remove(last);
            nodes.RemoveAt(nodes.Count - 1);
        }

        Add(domainEvent);
    }

    /// <summary>
    /// Removes all previous events of the same type (if present) and appends the new one to the end.
    /// Use when the latest version should overwrite all earlier ones.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserLastSeenUpdatedEvent</c> — only the most recent login is relevant, earlier ones are obsolete.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The new event to add or replace.</param>
    public void AddOrReplaceAll<T>(T domainEvent) where T : DomainEvent
    {
        var type = typeof(T);

        if (_eventsByType.TryGetValue(type, out var nodes))
        {
            foreach (var node in nodes)
            {
                _events.Remove(node);
            }
        }

        _eventsByType.Remove(type);
        Add(domainEvent);
    }

    /// <summary>
    /// Collects and returns all domain events in the order they were added.
    /// Intended to be called by infrastructure to dispatch events.
    /// </summary>
    /// <returns>List of domain events.</returns>
    public virtual IReadOnlyList<DomainEvent> Collect()
    {
        var collected = _events.ToList().AsReadOnly();
        return collected;
    }

    /// <summary>
    /// Clears all currently stored domain events.
    /// Typically called after events are dispatched.
    /// </summary>
    public void Clear()
    {
        _events.Clear();
        _eventsByType.Clear();
    }

    private void AddNode(Type type, LinkedListNode<DomainEvent> node)
    {
        if (!_eventsByType.TryGetValue(type, out var list))
        {
            list = [];
            _eventsByType[type] = list;
        }

        list.Add(node);
    }
}
