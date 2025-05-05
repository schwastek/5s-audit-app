using Domain.Exceptions;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.ErrorCodeTests;

public sealed class ErrorCodeComparableTests
{
    [Fact]
    public void Can_sort_error_codes()
    {
        var errorCode1 = new ErrorCode("A");
        var errorCode2 = new ErrorCode("b");
        var errorCode3 = new ErrorCode("c");
        var errorCode4 = new ErrorCode("D");

        var errorCodes = new List<ErrorCode>(4) { errorCode4, errorCode1, errorCode3, errorCode2 };
        errorCodes.Sort();

        Assert.True(errorCodes[0].Equals(errorCode1));
        Assert.True(errorCodes[1].Equals(errorCode2));
        Assert.True(errorCodes[2].Equals(errorCode3));
        Assert.True(errorCodes[3].Equals(errorCode4));
    }
}
