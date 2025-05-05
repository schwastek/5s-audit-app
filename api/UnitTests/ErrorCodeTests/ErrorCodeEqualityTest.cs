using Domain.Exceptions;
using Xunit;

namespace UnitTests.ErrorCodeTests;

public sealed class ErrorCodeEqualityTest
{
    private class DerivedErrorCode : ErrorCode
    {
        public string Message { get; }

        public DerivedErrorCode(string message, string code) : base(code)
        {
            Message = message;
        }
    }

    [Fact]
    public void Derived_error_code_is_not_equal()
    {
        var errorCode = new ErrorCode("AuditDoesNotExist");
        var derivedErrorCode = new DerivedErrorCode("ErrorMessage", "AuditDoesNotExist");

        Assert.False(errorCode.Equals(derivedErrorCode));
        Assert.False(derivedErrorCode.Equals(errorCode));
        Assert.False(errorCode == derivedErrorCode);
    }

    [Fact]
    public void Two_error_codes_of_the_same_content_are_equal()
    {
        var errorCode1 = new ErrorCode("AuditDoesNotExist");
        var errorCode2 = new ErrorCode("AuditDoesNotExist");

        Assert.True(errorCode1.Equals(errorCode2));
        Assert.True(errorCode1 == errorCode2);
        Assert.True(errorCode1.GetHashCode().Equals(errorCode2.GetHashCode()));
    }

    [Fact]
    public void Two_different_error_codes_are_not_equal()
    {
        var errorCode1 = new ErrorCode("AuditDoesNotExist");
        var errorCode2 = new ErrorCode("UserDoesNotExist");

        Assert.False(errorCode1.Equals(errorCode2));
        Assert.False(errorCode1 == errorCode2);
        Assert.False(errorCode1.GetHashCode().Equals(errorCode2.GetHashCode()));
    }

    [Fact]
    public void Two_error_codes_with_different_casing_are_equal()
    {
        var errorCode1 = new ErrorCode("AuditDoesNotExist");
        var errorCode2 = new ErrorCode("auditdoesnotexist");

        Assert.True(errorCode1.Equals(errorCode2));
        Assert.True(errorCode1 == errorCode2);
        Assert.True(errorCode1.GetHashCode().Equals(errorCode2.GetHashCode()));
    }
}
