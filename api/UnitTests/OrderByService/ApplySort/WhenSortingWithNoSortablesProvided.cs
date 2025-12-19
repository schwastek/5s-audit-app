using Infrastructure.OrderByService;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.OrderByService.ApplySort;

public sealed class When_applying_sort_with_null_sortables
{
    [Fact]
    public void Then_query_is_not_modified()
    {
        // Arrange
        var source = TestData.People;

        // Act
        var result = source.ApplySort(sortables: null).ToList();

        // Assert
        Assert.Equal(source.ToList(), result);
    }
}

public sealed class When_applying_sort_with_empty_sortables
{
    [Fact]
    public void Then_query_is_not_modified()
    {
        // Arrange
        var source = TestData.People;

        // Act
        var result = source.ApplySort(sortables: []).ToList();

        // Assert
        Assert.Equal(source.ToList(), result);
    }
}
