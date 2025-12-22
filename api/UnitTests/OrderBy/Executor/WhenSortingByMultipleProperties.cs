using Infrastructure.OrderBy;
using System.Linq;
using Xunit;

namespace UnitTests.OrderBy.Executor;

public sealed class When_sorting_by_multiple_properties_ascending
{
    [Fact]
    public void Then_secondary_sort_is_applied_with_thenby()
    {
        // Arrange
        var source = TestData.People;

        var instructions = new[]
        {
            new OrderByInstruction
            {
                PropertyName = nameof(Person.Age),
                SortDescending = false
            },
            new OrderByInstruction
            {
                PropertyName = nameof(Person.FirstName),
                SortDescending = false
            }
        };

        // Act
        var result = OrderByExecutor.ApplyOrderBy(source, instructions).ToList();

        // Assert
        Assert.Equal(
            ["Bob", "Alice", "Charlie", "Bob"],
            result.Select(p => p.FirstName)
        );
    }
}

public sealed class When_sorting_by_multiple_properties_with_mixed_directions
{
    [Fact]
    public void Then_sort_directions_are_applied_correctly()
    {
        // Arrange
        var source = TestData.People;

        var instructions = new[]
        {
            new OrderByInstruction
            {
                PropertyName = nameof(Person.FirstName),
                SortDescending = false
            },
            new OrderByInstruction
            {
                PropertyName = nameof(Person.Age),
                SortDescending = true
            }
        };

        // Act
        var result = OrderByExecutor.ApplyOrderBy(source, instructions).ToList();

        Assert.Equal(
            [30, 40, 25, 30],
            result.Select(p => p.Age)
        );
    }
}
