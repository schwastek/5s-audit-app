using Domain.Events;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Domain.Events;

public class DomainEventsTests
{
    [Fact]
    public void Should_AddEvent()
    {
        // Arrange
        var article = new Article();
        var tag1 = "1";
        var tag2 = "2";
        var category1 = "1";
        var category2 = "2";

        // Act
        article.AddTag(tag1);
        article.AddTag(tag2);
        article.RemoveTag(tag1);

        article.AddCategory(category1);
        article.AddCategory(category2);
        article.RemoveCategory(category1);

        // Assert
        var result = article.GetDomainEvents();
        Assert.Single(result);

        var expectedEvent = result[0] as ArticleEntityModifiedEvent;
        Assert.NotNull(expectedEvent);
        Assert.Equal(2, expectedEvent.ChangedEvents.Count);

        var expectedCollectionChangeEvent1 = expectedEvent.ChangedEvents[0] as TagsCollectionChangedEvent;
        Assert.NotNull(expectedCollectionChangeEvent1);
        Assert.Single(expectedCollectionChangeEvent1.AddedItems);
        Assert.Equal(tag2, expectedCollectionChangeEvent1.AddedItems[0], expectedCollectionChangeEvent1.Comparer);

        var expectedCollectionChangeEvent2 = expectedEvent.ChangedEvents[1] as CategoriesCollectionChangedEvent;
        Assert.NotNull(expectedCollectionChangeEvent2);
        Assert.Single(expectedCollectionChangeEvent2.AddedItems);
        Assert.Equal(category2, expectedCollectionChangeEvent2.AddedItems[0], expectedCollectionChangeEvent2.Comparer);
    }

    private class Article : IHaveDomainEvents
    {
        private readonly List<string> _tags = [];
        private readonly List<string> _categories = [];
        private readonly ArticleEvents _domainEvents = new ArticleEvents();

        public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
        public IReadOnlyCollection<string> Categories => _categories.AsReadOnly();

        public void AddTag(string tag)
        {
            _tags.Add(tag);
            _domainEvents.Add(new TagsCollectionChangedEvent(addedItems: [tag]));
        }

        public void RemoveTag(string tag)
        {
            _tags.Remove(tag);
            _domainEvents.Add(new TagsCollectionChangedEvent(removedItems: [tag]));
        }

        public void AddCategory(string category)
        {
            _categories.Add(category);
            _domainEvents.Add(new CategoriesCollectionChangedEvent(addedItems: [category]));
        }

        public void RemoveCategory(string category)
        {
            _categories.Remove(category);
            _domainEvents.Add(new CategoriesCollectionChangedEvent(removedItems: [category]));
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public IReadOnlyList<DomainEvent> GetDomainEvents()
        {
            return _domainEvents.Get();
        }
    }

    private class ArticleEvents : DomainEvents { }

    private class ArticleEntityModifiedEvent : EntityModifiedDomainEvent
    {
        public ArticleEntityModifiedEvent(ChangedEvent changedEvent) : base(changedEvent) { }
    }

    private class TagsCollectionChangedEvent : CollectionChangedEvent<string>
    {
        public TagsCollectionChangedEvent(IEnumerable<string>? addedItems = null, IEnumerable<string>? removedItems = null)
            : base(collectionName: nameof(Article.Tags), addedItems, removedItems, comparer: null)
        {
        }

        public override EntityModifiedDomainEvent AsDomainEvent()
        {
            return new ArticleEntityModifiedEvent(this);
        }

        public override TagsCollectionChangedEvent Clone(IEnumerable<string>? addedItems, IEnumerable<string>? removedItems)
        {
            return new TagsCollectionChangedEvent(addedItems, removedItems);
        }
    }

    private class CategoriesCollectionChangedEvent : CollectionChangedEvent<string>
    {
        public CategoriesCollectionChangedEvent(IEnumerable<string>? addedItems = null, IEnumerable<string>? removedItems = null)
            : base(collectionName: nameof(Article.Categories), addedItems, removedItems, comparer: null)
        {
        }

        public override EntityModifiedDomainEvent AsDomainEvent()
        {
            return new ArticleEntityModifiedEvent(this);
        }

        public override CategoriesCollectionChangedEvent Clone(IEnumerable<string>? addedItems, IEnumerable<string>? removedItems)
        {
            return new CategoriesCollectionChangedEvent(addedItems, removedItems);
        }
    }
}
