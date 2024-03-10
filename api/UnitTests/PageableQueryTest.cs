using Core.Pagination;
using Xunit;

namespace UnitTests;

public class PageableQueryTest
{
    [Fact]
    public void ShouldReturnDefaultPageNumber_WhenPassedNullPageNumber()
    {
        // Arrange & Act
        var query = new PageableQuery(pageNumber: null, pageSize: 1);

        // Assert
        Assert.Equal(PageableQuery.PageNumberDefault, query.PageNumber);
    }

    [Fact]
    public void ShouldReturnDefaultPageNumber_WhenPassedNegativePageNumber()
    {
        // Arrange & Act
        var query = new PageableQuery(pageNumber: -11, pageSize: 1);

        // Assert
        Assert.Equal(PageableQuery.PageNumberDefault, query.PageNumber);
    }

    [Fact]
    public void ShouldReturnDefaultPageSize_WhenPassedNullPageSize()
    {
        // Arrange & Act
        var query = new PageableQuery(pageNumber: 1, pageSize: null);

        // Assert
        Assert.Equal(PageableQuery.PageSizeDefault, query.PageSize);
    }

    [Fact]
    public void ShouldReturnMinPageSize_WhenPassedNegativePageSize()
    {
        // Arrange & Act
        var query = new PageableQuery(pageNumber: 1, pageSize: -11);

        // Assert
        Assert.Equal(PageableQuery.PageSizeMin, query.PageSize);
    }

    [Fact]
    public void ShouldReturnMaxPageSize_WhenPassedPageSizeGreaterThanLimit()
    {
        // Arrange & Act
        var query = new PageableQuery(pageNumber: 1, pageSize: 1111);

        // Assert
        Assert.Equal(PageableQuery.PageSizeMax, query.PageSize);
    }
}
