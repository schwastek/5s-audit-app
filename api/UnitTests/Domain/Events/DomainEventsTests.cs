using Domain.Events;
using Domain.Events.ChangeTracking;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Domain.Events;

public sealed class DomainEventsTests
{
    [Fact]
    public void Can_add_domain_events()
    {
        // Arrange
        var domainEvents = new DomainEvents();
        var event1 = new FakeDomainEvent();
        var event2 = new FakeDomainEvent();

        // Act
        domainEvents.Add(event1);
        domainEvents.Add(event2);
        var events = domainEvents.Collect();

        // Assert
        Assert.Equal(2, events.Count);
        Assert.Contains(event1, events);
        Assert.Contains(event2, events);
    }

    [Fact]
    public void Can_clear_domain_events()
    {
        // Arrange
        var domainEvents = new DomainEvents();
        var event1 = new FakeDomainEvent();

        // Act
        domainEvents.Add(event1);
        domainEvents.Clear();
        var events = domainEvents.Collect();

        // Assert
        Assert.Empty(events);
    }

    [Fact]
    public void Adds_entity_changed_event_only_once()
    {
        // Arrange
        var domainEvents = new DomainEvents();
        var change = new FakePropertyChanged(oldValue: "Old", newValue: "New");
        var changes = new List<MemberChanged>(1) { change };
        var firstEvent = new FakeEntityChangedEvent(changes);
        var secondEvent = new FakeEntityChangedEvent(changes);

        // Act
        domainEvents.Add(firstEvent);
        domainEvents.Add(secondEvent);
        var events = domainEvents.Collect();

        // Assert
        Assert.Single(events);
        Assert.Contains(firstEvent, events);
    }

    private class FakeDomainEvent : DomainEvent { }

    private class FakeEntityChangedEvent : EntityChangedEvent
    {
        public FakeEntityChangedEvent(IReadOnlyCollection<MemberChanged> changes) : base(changes) { }
    }

    private class FakePropertyChanged : PropertyChanged<string>
    {
        public FakePropertyChanged(string oldValue, string newValue)
            : base(propertyName: "Fake", oldValue, newValue, null) { }
    }
}
