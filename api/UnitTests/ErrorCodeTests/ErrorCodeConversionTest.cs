using Domain.Exceptions;
using Xunit;

namespace UnitTests.ErrorCodeTests;

public sealed class ErrorCodeConversionTest
{
    [Fact]
    public void Implicit_operator_converts_error_code_object_to_string()
    {
        ErrorCode errorCode = new("AuditDoesNotExist");
        string result = errorCode;

        Assert.Equal("AuditDoesNotExist", result);
    }

    [Fact]
    public void Explicit_operator_converts_string_to_error_code_object()
    {
        string code = "AuditDoesNotExist";
        ErrorCode result = (ErrorCode)code;

        Assert.Equal("AuditDoesNotExist", result.Value);
    }
}
