using Infrastructure.OrderBy;
using Xunit;

namespace UnitTests.OrderBy.Parser;

public sealed class When_creating_order_by_instructions_from_invalid_query
{
    [Fact]
    public void Then_extra_words_after_desc_are_ignored()
    {
        // Arrange
        var query = "author desc foo";

        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Single(result);
        Assert.Equal("author", result[0].PropertyName);
        Assert.True(result[0].SortDescending);
    }

    [Fact]
    public void Then_empty_segment_is_skipped()
    {
        // Arrange
        var query = "author asc, , created desc";

        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("author", result[0].PropertyName);
        Assert.False(result[0].SortDescending);

        Assert.Equal("created", result[1].PropertyName);
        Assert.True(result[1].SortDescending);
    }

    [Fact]
    public void Then_consecutive_commas_are_handled_gracefully()
    {
        // Arrange
        var query = "author,,created,,area desc";

        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("author", result[0].PropertyName);
        Assert.False(result[0].SortDescending);

        Assert.Equal("created", result[1].PropertyName);
        Assert.False(result[1].SortDescending);

        Assert.Equal("area", result[2].PropertyName);
        Assert.True(result[2].SortDescending);
    }

    [Fact]
    public void Then_property_with_no_desc_or_asc_defaults_to_ascending()
    {
        // Arrange
        var query = "created";

        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Single(result);
        Assert.Equal("created", result[0].PropertyName);
        Assert.False(result[0].SortDescending);
    }
}
