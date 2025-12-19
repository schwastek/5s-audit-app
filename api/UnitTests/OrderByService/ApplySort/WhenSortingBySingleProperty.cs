using Infrastructure.OrderByService;
using System.Linq;
using Xunit;

namespace UnitTests.OrderByService.ApplySort;

public sealed class When_sorting_by_single_property_ascending
{
    [Fact]
    public void Then_items_are_sorted_in_ascending_order()
    {
        // Arrange
        var source = TestData.People;

        var sortables = new[]
        {
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
            ["Alice", "Bob", "Bob", "Charlie"],
            result.Select(p => p.FirstName)
        );
    }
}

public sealed class When_sorting_by_single_property_descending
{
    [Fact]
    public void Then_items_are_sorted_in_descending_order()
    {
        // Arrange
        var source = TestData.People;

        var sortables = new[]
        {
            new Sortable
            {
                PropertyName = nameof(Person.Age),
                SortDescending = true
            }
        };

        // Act
        var result = source.ApplySort(sortables).ToList();

        // Assert
        Assert.Equal(
            [40, 30, 30, 25],
            result.Select(p => p.Age)
        );
    }
}
