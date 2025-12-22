using Infrastructure.OrderBy;
using System.Linq;
using Xunit;

namespace UnitTests.OrderBy.Executor;

public sealed class When_applying_sort_with_null_instructions
{
    [Fact]
    public void Then_query_is_not_modified()
    {
        // Arrange
        var source = TestData.People;

        // Act
        var result = OrderByExecutor.ApplyOrderBy(source, instructions: null).ToList();

        // Assert
        Assert.Equal(source.ToList(), result);
    }
}

public sealed class When_applying_sort_with_empty_instructions
{
    [Fact]
    public void Then_query_is_not_modified()
    {
        // Arrange
        var source = TestData.People;

        // Act
        var result = OrderByExecutor.ApplyOrderBy(source, instructions: []).ToList();

        // Assert
        Assert.Equal(source.ToList(), result);
    }
}
