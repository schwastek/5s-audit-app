using Domain.Events.ChangeTracking;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Domain.Events;

public sealed class CollectionChangedTests
{
    [Fact]
    public void Merge_updates_previous_change()
    {
        // Arrange
        var category1 = new Category("Articles");
        var category2 = new Category("News");

        var change1 = new CategoriesCollectionChangedEvent(added: [category1]);
        var change2 = new CategoriesCollectionChangedEvent(removed: [category2]);

        // Act
        change1.MergeWith(change2);

        // Assert
        Assert.Single(change1.AddedItems);
        Assert.Single(change1.RemovedItems);
        Assert.Contains(category1, change1.AddedItems);
        Assert.Contains(category2, change1.RemovedItems);
    }

    [Fact]
    public void Merge_throws_when_property_names_differ()
    {
        // Arrange
        var change1 = new FakeCollectionChanged(collectionName: "Fake1");
        var change2 = new FakeCollectionChanged(collectionName: "Fake2");

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => change1.MergeWith(change2));

        // Assert
        Assert.Contains("Merge failed", ex.Message);
    }

    [Fact]
    public void No_change_when_added_and_removed_are_equal()
    {
        // Arrange
        var category = new Category("News");
        var change = new CategoriesCollectionChangedEvent(added: [category], removed: [category]);

        // Act
        var noChanges = change.NoChanges();

        // Assert
        Assert.True(noChanges);
    }

    [Fact]
    public void No_change_when_added_and_removed_are_equal_with_duplicates()
    {
        // Arrange
        var category = new Category("News");
        var change = new CategoriesCollectionChangedEvent(added: [category, category], removed: [category, category, category]);

        // Act
        var noChanges = change.NoChanges();

        // Assert
        Assert.True(noChanges);
    }

    [Fact]
    public void Single_change_when_same_item_added_or_removed()
    {
        // Arrange
        var category1 = new Category("News");
        var category2 = new Category("Articles");

        var change1 = new CategoriesCollectionChangedEvent(added: [category1], removed: [category2]);
        var change2 = new CategoriesCollectionChangedEvent(added: [category1], removed: [category2]);

        // Act
        change1.MergeWith(change2);

        // Assert
        Assert.Single(change1.AddedItems);
        Assert.Single(change1.RemovedItems);
        Assert.Contains(category1, change1.AddedItems);
        Assert.Contains(category2, change1.RemovedItems);
    }

    [Fact]
    public void No_change_when_added_and_removed_cancel_out()
    {
        // Arrange
        var category1 = new Category("News");
        var category2 = new Category("Articles");

        var change1 = new CategoriesCollectionChangedEvent(added: [category1], removed: []);
        var change2 = new CategoriesCollectionChangedEvent(added: [category2], removed: [category1]);
        var change3 = new CategoriesCollectionChangedEvent(added: [], removed: [category2]);

        // Act
        change1.MergeWith(change2).MergeWith(change3);
        var noChanges = change1.NoChanges();

        // Assert
        Assert.True(noChanges);
    }

    [Fact]
    public void Merge_with_empty_change_does_not_alter_existing_change()
    {
        // Arrange
        var category = new Category("News");
        var change1 = new CategoriesCollectionChangedEvent(added: [category]);
        var change2 = new CategoriesCollectionChangedEvent();

        // Act
        change1.MergeWith(change2);

        // Assert
        Assert.Single(change1.AddedItems);
        Assert.Contains(category, change1.AddedItems);
        Assert.Empty(change1.RemovedItems);
    }

    private class FakeCollectionChanged : CollectionChanged<string>
    {
        public FakeCollectionChanged(string collectionName, IEnumerable<string>? added = null, IEnumerable<string>? removed = null)
            : base(collectionName, added, removed, null) { }
    }

    private class Category
    {
        public string Name { get; }

        public Category(string name)
        {
            Name = name;
        }
    }

    private class CategoriesCollectionChangedEvent : CollectionChanged<Category>
    {
        public CategoriesCollectionChangedEvent(IEnumerable<Category>? added = null, IEnumerable<Category>? removed = null)
            : base(collectionName: "Categories", added, removed, comparer: new CategoryComparer()) { }
    }

    private class CategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category? x, Category? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;

            if (x.GetType() != y.GetType()) return false;

            if (string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(Category obj)
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
        }
    }
}
