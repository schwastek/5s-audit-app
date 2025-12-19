using Infrastructure.OrderByService;
using System.Linq;
using Xunit;

namespace UnitTests.OrderByService.ApplySort;

public sealed class When_sorting_by_multiple_properties_ascending
{
    [Fact]
    public void Then_secondary_sort_is_applied_with_thenby()
    {
        // Arrange
        var source = TestData.People;

        var sortables = new[]
        {
            new Sortable
            {
                PropertyName = nameof(Person.Age),
                SortDescending = false
            },
            new Sortable
            {
                PropertyName = nameof(Person.FirstName),
                SortDescending = false
            }
        };

        // Act
        var result = source.ApplySort(sortables).ToList();

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

        var sortables = new[]
        {
            new Sortable
            {
                PropertyName = nameof(Person.FirstName),
                SortDescending = false
            },
            new Sortable
            {
                PropertyName = nameof(Person.Age),
                SortDescending = true
            }
        };

        // Act
        var result = source.ApplySort(sortables).ToList();

        Assert.Equal(
            [30, 40, 25, 30],
            result.Select(p => p.Age)
        );
    }
}
