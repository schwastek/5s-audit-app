using Domain.Events.ChangeTracking;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Domain.Events;

public sealed class PropertyChangedTests
{
    [Fact]
    public void Merge_updates_previous_change()
    {
        // Arrange
        var author1 = new Author("John", "Doe");
        var author2 = new Author("Jane", "Doe");

        var change1 = new AuthorPropertyChanged(oldValue: null, newValue: author1);
        var change2 = new AuthorPropertyChanged(oldValue: author1, newValue: author2);

        // Act
        change1.MergeWith(change2);

        // Assert
        Assert.Null(change1.OldValue);
        Assert.Equal(author2, change1.NewValue, new AuthorComparer());
    }

    [Fact]
    public void Merge_throws_when_property_names_differ()
    {
        // Arrange
        var change1 = new FakePropertyChanged(propertyName: "Fake1", oldValue: null, newValue: null);
        var change2 = new FakePropertyChanged(propertyName: "Fake2", oldValue: null, newValue: null);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => change1.MergeWith(change2));

        // Assert
        Assert.Contains("Merge failed", ex.Message);
    }

    [Fact]
    public void No_change_when_old_and_new_values_are_equal()
    {
        // Arrange
        var author = new Author("John", "Doe");
        var change = new AuthorPropertyChanged(oldValue: author, newValue: author);

        // Act
        var noChanges = change.NoChanges();

        // Assert
        Assert.True(noChanges);
    }

    [Fact]
    public void No_change_when_old_and_new_values_cancel_out()
    {
        // Arrange
        var author1 = new Author("John", "Doe");
        var author2 = new Author("Jane", "Doe");

        var change1 = new AuthorPropertyChanged(oldValue: null, newValue: author1);
        var change2 = new AuthorPropertyChanged(oldValue: author1, newValue: author2);
        var change3 = new AuthorPropertyChanged(oldValue: author2, newValue: null);

        // Act
        change1.MergeWith(change2).MergeWith(change3);
        var noChanges = change1.NoChanges();

        // Assert
        Assert.True(noChanges);
    }

    private class FakePropertyChanged : PropertyChanged<string>
    {
        public FakePropertyChanged(string propertyName, string? oldValue, string? newValue)
            : base(propertyName, oldValue, newValue, null) { }
    }

    private class AuthorPropertyChanged : PropertyChanged<Author>
    {
        public AuthorPropertyChanged(Author? oldValue, Author? newValue)
            : base(propertyName: "Author", oldValue, newValue, new AuthorComparer()) { }
    }

    private class Author
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }

    private class AuthorComparer : IEqualityComparer<Author?>
    {
        public bool Equals(Author? x, Author? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            if (x.GetType() != y.GetType()) return false;

            return string.Equals(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(x.LastName, y.LastName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Author obj)
        {
            int hashFirstName = StringComparer.OrdinalIgnoreCase.GetHashCode(obj.FirstName);
            int hashLastName = StringComparer.OrdinalIgnoreCase.GetHashCode(obj.LastName);

            return HashCode.Combine(hashFirstName, hashLastName);
        }
    }
}
