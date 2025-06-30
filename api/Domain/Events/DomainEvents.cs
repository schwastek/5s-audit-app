using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Events;

/// <summary>
/// Stores and manages domain events.
/// </summary>
public class DomainEvents
{
    private readonly List<DomainEvent> _events = [];
    private readonly Dictionary<Type, int> _eventTypeIndex = [];

    /// <summary>
    /// Adds a domain event to the list, even if another event of the same type already exists.
    /// Use when all occurrences of the event matter and should be captured independently.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserLoggedInEvent</c> — users can log in multiple times, each login is significant.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The event instance to add.</param>
    public void Add<T>(T domainEvent) where T : DomainEvent
    {
        // Always add, even if type already exists (duplicates allowed).
        _events.Add(domainEvent);
        // Update index to latest occurrence (for consistency).
        _eventTypeIndex[typeof(T)] = _events.Count - 1;
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

        if (!_eventTypeIndex.ContainsKey(type))
        {
            _events.Add(domainEvent);
            _eventTypeIndex[type] = _events.Count - 1;
        }
    }

    /// <summary>
    /// Adds a domain event or replaces the previous event of the same type.
    /// Use when the latest version should overwrite any earlier one; order does not matter.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserLastLoginUpdatedEvent</c> — only the most recent login is relevant, earlier ones are obsolete.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The new event to add or replace.</param>
    public void AddOrReplace<T>(T domainEvent) where T : DomainEvent
    {
        var type = typeof(T);

        if (_eventTypeIndex.TryGetValue(type, out int index))
        {
            // Replace.
            _events[index] = domainEvent;
        }
        else
        {
            _events.Add(domainEvent);
            _eventTypeIndex[type] = _events.Count - 1;
        }
    }

    /// <summary>
    /// Removes the previous event of the same type (if present) and appends the new one to the end.
    /// Keeps event order correct while ensuring only the latest version remains; order matters.
    /// </summary>
    /// <remarks>
    /// Example: <c>UserSessionStartedEvent</c> — only the latest session matters, but ordering with other events is important.
    /// </remarks>
    /// <typeparam name="T">Type of the domain event.</typeparam>
    /// <param name="domainEvent">The new event to add.</param>
    public void AddOrAppend<T>(T domainEvent) where T : DomainEvent
    {
        var type = typeof(T);

        if (_eventTypeIndex.TryGetValue(type, out int index))
        {
            _events.RemoveAt(index);

            // Update indices for events after the removed one.
            foreach (var key in _eventTypeIndex.Keys.ToList())
            {
                if (_eventTypeIndex[key] > index)
                {
                    _eventTypeIndex[key]--;
                }
            }
        }

        _events.Add(domainEvent);
        _eventTypeIndex[type] = _events.Count - 1;
    }

    /// <summary>
    /// Collects and returns all domain events in the order they were added.
    /// Intended to be called by infrastructure to dispatch events.
    /// </summary>
    /// <returns>List of domain events.</returns>
    public virtual IReadOnlyList<DomainEvent> Collect()
    {
        return _events.AsReadOnly();
    }

    /// <summary>
    /// Clears all currently stored domain events.
    /// Typically called after events are dispatched.
    /// </summary>
    public void Clear()
    {
        _events.Clear();
        _eventTypeIndex.Clear();
    }
}
