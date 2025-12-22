using Infrastructure.OrderBy;
using Xunit;

namespace UnitTests.OrderBy.Service;

public sealed class When_resolving_order_by_query
{
    private readonly IOrderByService<FakeEntity> _orderByService;

    public When_resolving_order_by_query()
    {
        var mapping = new FakeEntityOrderByMapConfiguration();
        _orderByService = new OrderByService<FakeEntity>(mapping);
    }

    [Fact]
    public void Should_resolve_order_instructions_for_property_ascending()
    {
        // Arrange
        var orderByQuery = "author asc";

        // Act
        var result = _orderByService.Resolve(orderByQuery);

        // Assert
        Assert.Single(result);
        Assert.Equal(nameof(FakeEntity.Author), result[0].PropertyName);
        Assert.False(result[0].SortDescending);
    }

    [Fact]
    public void Should_resolve_order_instructions_for_property_descending()
    {
        // Arrange
        var orderByQuery = "author desc";

        // Act
        var result = _orderByService.Resolve(orderByQuery);

        // Assert
        Assert.Single(result);
        Assert.Equal(nameof(FakeEntity.Author), result[0].PropertyName);
        Assert.True(result[0].SortDescending);
    }

    [Fact]
    public void Should_resolve_order_instructions_in_order()
    {
        // Arrange
        var orderByQuery = "author, fullname, age";

        // Act
        var result = _orderByService.Resolve(orderByQuery);

        // Assert
        var expectedInstructions = new OrderByInstruction[]
        {
            new() { PropertyName = "Author" },
            new() { PropertyName = "FirstName" },
            new() { PropertyName = "LastName" },
            new() { PropertyName = "DateOfBirth", SortDescending = true },
        };

        Assert.Equal(expectedInstructions, result);
    }

    [Fact]
    public void Should_resolve_order_instructions_for_reversed_property()
    {
        // Arrange
        var orderByQuery = "age asc";

        // Act
        var result = _orderByService.Resolve(orderByQuery);

        // Assert
        Assert.Single(result);
        Assert.Equal(nameof(FakeEntity.DateOfBirth), result[0].PropertyName);
        Assert.True(result[0].SortDescending);
    }

    [Fact]
    public void Should_resolve_order_instructions_for_single_property_with_multiple_targets()
    {
        // Arrange
        var orderByQuery = "fullname asc";

        // Act
        var result = _orderByService.Resolve(orderByQuery);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(nameof(FakeEntity.FirstName), result[0].PropertyName);
        Assert.False(result[0].SortDescending);

        Assert.Equal(nameof(FakeEntity.LastName), result[1].PropertyName);
        Assert.False(result[1].SortDescending);
    }

    [Fact]
    public void Should_throw_for_unsupported_property()
    {
        // Arrange
        var orderByQuery = "nonexistentproperty asc";

        // Act & Assert
        Assert.Throws<UnsupportedOrderByException>(() => _orderByService.Resolve(orderByQuery));
    }
}
