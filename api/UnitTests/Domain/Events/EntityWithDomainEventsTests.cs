using Domain.Events;
using Domain.Events.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Domain.Events;

public sealed class EntityWithDomainEventsTests
{
    [Fact]
    public void Changing_primitive_property_emits_expected_event()
    {
        // Arrange
        var article = new Article();
        article.SetTitle("Initial");
        article.SetTitle("Updated");

        // Act
        var events = article.CollectDomainEvents();

        // Assert
        var expectedEvents = events.OfType<ArticleEntityChangedEvent>();
        var expectedEvent = Assert.Single(expectedEvents);

        Assert.Single(expectedEvent.Changes);
        var change = Assert.IsType<ArticleTitlePropertyChanged>(expectedEvent.Changes.First());

        Assert.Null(change.OldValue);
        Assert.Equal("Updated", change.NewValue);
    }

    [Fact]
    public void Changing_complex_property_emits_expected_event()
    {
        // Arrange
        var article = new Article();
        var author1 = new Author("John", "Doe");
        var author2 = new Author("Jane", "Doe");

        article.SetAuthor(author1);
        article.SetAuthor(author2);

        // Act
        var events = article.CollectDomainEvents();

        // Assert
        var expectedEvents = events.OfType<ArticleEntityChangedEvent>();
        var expectedEvent = Assert.Single(expectedEvents);

        Assert.Single(expectedEvent.Changes);
        var change = Assert.IsType<ArticleAuthorPropertyChanged>(expectedEvent.Changes.First());

        Assert.Null(change.OldValue);
        Assert.Equal(author2, change.NewValue);
    }

    [Fact]
    public void Changing_primitive_collection_emits_expected_event()
    {
        // Arrange
        var article = new Article();

        article.AddTag("dotnet");
        article.RemoveTag("dotnet");
        article.AddTag("js");

        // Act
        var events = article.CollectDomainEvents();

        // Assert
        var expectedEvents = events.OfType<ArticleEntityChangedEvent>();
        var expectedEvent = Assert.Single(expectedEvents);

        Assert.Single(expectedEvent.Changes);
        var change = Assert.IsType<ArticleTagsCollectionChanged>(expectedEvent.Changes.First());

        Assert.Contains("js", change.AddedItems);
        Assert.Empty(change.RemovedItems);
    }

    [Fact]
    public void Changing_complex_collection_emits_expected_event()
    {
        // Arrange
        var article = new Article();

        var category1 = new Category("A");
        var category2 = new Category("B");

        article.AddCategory(category1);
        article.AddCategory(category2);
        article.RemoveCategory(category1);

        // Act
        var events = article.CollectDomainEvents();

        // Assert
        var expectedEvents = events.OfType<ArticleEntityChangedEvent>();
        var expectedEvent = Assert.Single(expectedEvents);

        Assert.Single(expectedEvent.Changes);
        var change = Assert.IsType<ArticleCategoriesCollectionChanged>(expectedEvent.Changes.First());

        Assert.Contains(category2, change.AddedItems);
        Assert.Empty(change.RemovedItems);
    }

    [Fact]
    public void Clear_domain_events_removes_all_events()
    {
        // Arrange
        var article = new Article();
        var author = new Author("John", "Doe");
        var category = new Category("A");
        article.SetTitle("Initial");
        article.AddTag("dotnet");
        article.SetAuthor(author);
        article.AddCategory(category);

        // Act
        article.ClearDomainEvents();
        var events = article.CollectDomainEvents();

        // Assert
        Assert.Empty(events);
    }

    [Fact]
    public void No_events_emitted_when_mutually_exclusive_changes()
    {
        // Arrange
        var article = new Article();

        // Act
        article.SetTitle("Initial");
        article.SetTitle(null);

        var author = new Author("John", "Doe");
        article.SetAuthor(author);
        article.SetAuthor(null);

        article.AddTag("dotnet");
        article.RemoveTag("dotnet");
        article.AddTag("js");
        article.RemoveTag("js");

        var category1 = new Category("A");
        var category2 = new Category("B");
        article.AddCategory(category1);
        article.AddCategory(category2);
        article.RemoveCategory(category1);
        article.RemoveCategory(category2);

        var events = article.CollectDomainEvents();

        // Assert
        Assert.Empty(events);
    }

    private class Article : IHaveDomainEvents
    {
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;

        private string? _title;
        public string? Title => _title;

        private Author? _author;
        public Author? Author => _author;

        private readonly List<Category> _categories = [];
        public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

        private readonly List<string> _tags = [];
        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();

        private readonly DomainEvents _events = new();
        private readonly EntityChangeTracker _tracker = new();

        public void SetTitle(string? title)
        {
            _tracker.Add(new ArticleTitlePropertyChanged(newValue: title, oldValue: _title));
            _title = title;
        }

        public void SetAuthor(Author? author)
        {
            _tracker.Add(new ArticleAuthorPropertyChanged(newValue: author, oldValue: _author));
            _author = author;
        }

        public void AddCategory(Category category)
        {
            _categories.Add(category);
            _tracker.Add(new ArticleCategoriesCollectionChanged(added: [category]));
        }

        public void RemoveCategory(Category category)
        {
            _categories.Remove(category);
            _tracker.Add(new ArticleCategoriesCollectionChanged(removed: [category]));
        }

        public void AddTag(string tag)
        {
            _tags.Add(tag);
            _tracker.Add(new ArticleTagsCollectionChanged(added: [tag]));
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
            _tracker.Add(new ArticleTagsCollectionChanged(removed: [tag]));
        }

        public void ClearDomainEvents()
        {
            _events.Clear();
            _tracker.Clear();
        }

        public IReadOnlyList<DomainEvent> CollectDomainEvents()
        {
            if (_tracker.HasChanges)
            {
                _events.Add(new ArticleEntityChangedEvent(_id, _tracker.Changes));
            }

            return _events.Collect();
        }
    }

    private class ArticleEntityChangedEvent : EntityChangedEvent
    {
        public Guid Id { get; }

        public ArticleEntityChangedEvent(Guid id, IReadOnlyCollection<MemberChanged> changes) : base(changes)
        {
            Id = id;
        }
    }

    private class ArticleTitlePropertyChanged : PropertyChanged<string>
    {
        public ArticleTitlePropertyChanged(string? oldValue = null, string? newValue = null)
            : base(propertyName: nameof(Article.Title), oldValue, newValue, comparer: null) { }
    }

    private class ArticleAuthorPropertyChanged : PropertyChanged<Author>
    {
        public ArticleAuthorPropertyChanged(Author? oldValue = null, Author? newValue = null)
            : base(propertyName: nameof(Article.Author), oldValue, newValue, comparer: new AuthorComparer()) { }
    }

    private class ArticleTagsCollectionChanged : CollectionChanged<string>
    {
        public ArticleTagsCollectionChanged(IEnumerable<string>? added = null, IEnumerable<string>? removed = null)
            : base(collectionName: nameof(Article.Tags), added, removed, comparer: null) { }
    }

    private class ArticleCategoriesCollectionChanged : CollectionChanged<Category>
    {
        public ArticleCategoriesCollectionChanged(IEnumerable<Category>? added = null, IEnumerable<Category>? removed = null)
            : base(collectionName: nameof(Article.Categories), added, removed, comparer: new CategoryComparer()) { }
    }

    private class Category
    {
        public string Name { get; }

        public Category(string name)
        {
            Name = name;
        }
    }

    private class CategoryComparer : IEqualityComparer<Category>
    {
        public bool Equals(Category? x, Category? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            if (x.GetType() != y.GetType()) return false;

            return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Category obj)
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
        }
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

    private class AuthorComparer : IEqualityComparer<Author>
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
