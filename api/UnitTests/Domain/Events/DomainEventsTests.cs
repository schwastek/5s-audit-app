using Domain.Events;
using Xunit;

namespace UnitTests.Domain.Events;

public sealed class DomainEventsTests
{
    [Fact]
    public void Add_should_add_multiple_of_same_type()
    {
        // Arrange
        var events = new DomainEvents();

        // Act
        events.Add(new FakeDomainEvent { Data = "A" });
        events.Add(new FakeDomainEvent { Data = "B" });

        var collected = events.Collect();

        // Assert
        Assert.Equal(2, collected.Count);

        var first = Assert.IsType<FakeDomainEvent>(collected[0]);
        Assert.Equal("A", first.Data);

        var second = Assert.IsType<FakeDomainEvent>(collected[1]);
        Assert.Equal("B", second.Data);
    }

    [Fact]
    public void AddOnce_should_only_add_once_per_type()
    {
        // Arrange
        var events = new DomainEvents();

        // Act
        events.AddOnce(new FakeDomainEvent { Data = "Initial" });
        events.AddOnce(new FakeDomainEvent { Data = "Ignored" });

        var collected = events.Collect();

        // Assert
        Assert.Single(collected);

        var only = Assert.IsType<FakeDomainEvent>(collected[0]);
        Assert.Equal("Initial", only.Data);
    }

    [Fact]
    public void AddOrReplaceLast_should_remove_single_previous_and_add_to_end()
    {
        // Arrange
        var events = new DomainEvents();

        // Act
        events.Add(new FakeDomainEvent { Data = "First" });
        events.Add(new FakeDomainEvent { Data = "Second" });
        events.AddOrReplaceLast(new FakeDomainEvent { Data = "Third" });
        events.AddOrReplaceLast(new FakeDomainEvent { Data = "Fourth" });

        var collected = events.Collect();

        // Assert
        Assert.Equal(2, collected.Count);

        var last = Assert.IsType<FakeDomainEvent>(collected[1]);
        Assert.Equal("Fourth", last.Data);
    }

    [Fact]
    public void AddOrReplaceAll_should_remove_all_previous_and_add_to_end()
    {
        // Arrange
        var events = new DomainEvents();

        // Act
        events.Add(new FakeDomainEvent { Data = "First" });
        events.Add(new FakeDomainEvent { Data = "Second" });
        events.Add(new FakeDomainEvent { Data = "Third" });

        events.AddOrReplaceAll(new FakeDomainEvent { Data = "Fourth" });

        var collected = events.Collect();

        // Assert
        Assert.Single(collected);

        var last = Assert.IsType<FakeDomainEvent>(collected[0]);
        Assert.Equal("Fourth", last.Data);
    }


    [Fact]
    public void Clear_should_remove_all_events()
    {
        // Arrange
        var events = new DomainEvents();

        // Act
        events.Add(new FakeDomainEvent { Data = "Some" });
        events.Clear();

        var collected = events.Collect();

        // Assert
        Assert.Empty(collected);
    }

    [Fact]
    public void Collect_should_return_events_in_correct_order()
    {
        // Arrange
        var events = new DomainEvents();

        // Act
        events.Add(new FakeDomainEvent { Data = "A" });
        events.Add(new FakeDomainEvent { Data = "B" });
        events.Add(new FakeDomainEvent { Data = "C" });

        var collected = events.Collect();

        // Assert
        Assert.Equal(3, collected.Count);

        var first = Assert.IsType<FakeDomainEvent>(collected[0]);
        Assert.Equal("A", first.Data);

        var second = Assert.IsType<FakeDomainEvent>(collected[1]);
        Assert.Equal("B", second.Data);

        var third = Assert.IsType<FakeDomainEvent>(collected[2]);
        Assert.Equal("C", third.Data);
    }

    private class FakeDomainEvent : DomainEvent
    {
        public string? Data { get; set; }
    }
}
