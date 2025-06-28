using Domain.Events.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Domain.Events;

public sealed class EntityChangeTrackerTests
{
    [Fact]
    public void Add_new_property_changed_adds_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change = new FakePropertyChanged("Fake", "Old", "New");

        // Act
        tracker.Add(change);

        // Assert
        Assert.True(tracker.HasChanges);
        Assert.Single(tracker.Changes);
        Assert.Equal(change, tracker.Changes.First());
    }

    [Fact]
    public void Add_existing_property_changed_updates_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change1 = new FakePropertyChanged("Fake", "First", "Second");
        var change2 = new FakePropertyChanged("Fake", "Second", "Third");

        // Act
        tracker.Add(change1);
        tracker.Add(change2);

        // Assert
        Assert.True(tracker.HasChanges);
        Assert.Single(tracker.Changes);
        var change = tracker.Changes.FirstOrDefault() as FakePropertyChanged;
        Assert.NotNull(change);
        Assert.Equal("First", change.OldValue);
        Assert.Equal("Third", change.NewValue);
    }

    [Fact]
    public void Add_new_collection_changed_adds_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change = new FakeCollectionChanged("Fake", added: ["New"], removed: null);

        // Act
        tracker.Add(change);

        // Assert
        Assert.True(tracker.HasChanges);
        Assert.Single(tracker.Changes);
        Assert.Equal(change, tracker.Changes.First());
    }

    [Fact]
    public void Add_existing_collection_changed_updates_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change1 = new FakeCollectionChanged("Fake", added: ["1"], removed: ["2"]);
        var change2 = new FakeCollectionChanged("Fake", added: ["3"], removed: ["4"]);

        // Act
        tracker.Add(change1);
        tracker.Add(change2);

        // Assert
        Assert.True(tracker.HasChanges);
        Assert.Single(tracker.Changes);
        var change = tracker.Changes.FirstOrDefault() as FakeCollectionChanged;
        Assert.NotNull(change);
        Assert.Contains("1", change.AddedItems);
        Assert.Contains("3", change.AddedItems);
        Assert.Contains("2", change.RemovedItems);
        Assert.Contains("4", change.RemovedItems);
    }

    [Fact]
    public void Add_new_empty_property_changed_results_in_no_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change = new FakePropertyChanged("Fake", oldValue: "1", newValue: "1");

        // Act
        tracker.Add(change);

        // Assert
        Assert.False(tracker.HasChanges);
        Assert.Empty(tracker.Changes);
    }

    [Fact]
    public void Add_new_empty_collection_changed_results_in_no_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change = new FakeCollectionChanged("Fake", added: ["1"], removed: ["1"]);

        // Act
        tracker.Add(change);

        // Assert
        Assert.False(tracker.HasChanges);
        Assert.Empty(tracker.Changes);
    }

    [Fact]
    public void Add_property_changed_that_cancels_out_previous_change_results_in_no_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change1 = new FakePropertyChanged("Fake", oldValue: "1", newValue: "2");
        var change2 = new FakePropertyChanged("Fake", oldValue: "2", newValue: "1");

        // Act
        tracker.Add(change1);
        tracker.Add(change2);

        // Assert
        Assert.False(tracker.HasChanges);
        Assert.Empty(tracker.Changes);
    }

    [Fact]
    public void Add_collection_changed_that_cancels_out_previous_change_results_in_no_change()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var change1 = new FakeCollectionChanged("Fake", added: ["1"], removed: ["2"]);
        var change2 = new FakeCollectionChanged("Fake", added: ["2"], removed: ["1"]);

        // Act
        tracker.Add(change1);
        tracker.Add(change2);

        // Assert
        Assert.False(tracker.HasChanges);
        Assert.Empty(tracker.Changes);
    }

    [Fact]
    public void Add_throws_when_merging_different_types_for_same_member()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var propertyChange = new FakePropertyChanged("Fake", "Old", "New");
        var collectionChange = new FakeCollectionChanged("Fake", added: ["New"], removed: null);

        // Act
        tracker.Add(propertyChange);
        var ex = Assert.Throws<InvalidOperationException>(() => tracker.Add(collectionChange));

        // Assert
        Assert.Contains("Change already exists for member 'Fake'", ex.Message);
    }

    [Fact]
    public void Clear_removes_all_changes()
    {
        // Arrange
        var tracker = new EntityChangeTracker();
        var propertyChange = new FakePropertyChanged("FakeProperty", "Old", "New");
        var collectionChange = new FakeCollectionChanged("FakeCollection", added: ["New"], removed: null);
        tracker.Add(propertyChange);
        tracker.Add(collectionChange);

        // Act
        tracker.Clear();

        Assert.False(tracker.HasChanges);
        Assert.Empty(tracker.Changes);
    }

    private class FakePropertyChanged : PropertyChanged<string>
    {
        public FakePropertyChanged(string propertyName, string oldValue, string newValue)
            : base(propertyName, oldValue, newValue, null) { }
    }

    private class FakeCollectionChanged : CollectionChanged<string>
    {
        public FakeCollectionChanged(string collectionName, IEnumerable<string>? added, IEnumerable<string>? removed)
            : base(collectionName, added, removed, null) { }
    }
}
