using Infrastructure.OrderBy;
using Xunit;

namespace UnitTests.OrderBy.Parser;

public sealed class When_creating_order_by_instructions_from_query
{
    [Theory]
    [InlineData("author asc", "author", false)]
    [InlineData("author desc", "author", true)]
    [InlineData("created", "created", false)]
    public void Then_single_parameter_is_parsed_correctly(string query, string expectedProperty, bool expectedDescending)
    {
        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Single(result);
        Assert.Equal(expectedProperty, result[0].PropertyName);
        Assert.Equal(expectedDescending, result[0].SortDescending);
    }

    [Fact]
    public void Then_multiple_instructions_are_parsed_correctly()
    {
        // Arrange
        var query = "author asc, created desc, area";

        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Equal(3, result.Count);

        Assert.Equal("author", result[0].PropertyName);
        Assert.False(result[0].SortDescending);

        Assert.Equal("created", result[1].PropertyName);
        Assert.True(result[1].SortDescending);

        Assert.Equal("area", result[2].PropertyName);
        Assert.False(result[2].SortDescending);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Then_empty_or_whitespace_query_returns_empty_list(string query)
    {
        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Then_query_with_extra_spaces_is_trimmed_correctly()
    {
        // Arrange
        var query = "  author   asc  ,  created   desc  ";

        // Act
        var result = OrderByParser.Parse(query);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal("author", result[0].PropertyName);
        Assert.False(result[0].SortDescending);

        Assert.Equal("created", result[1].PropertyName);
        Assert.True(result[1].SortDescending);
    }
}
