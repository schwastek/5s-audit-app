using System;

namespace Domain.Exceptions;

/// <summary>
/// Represents an error code, encapsulating a string value for use in error handling.
/// Provides comparisons based on the error code string value.
/// Prevents ambiguity between string parameters like `errorCode` and `errorMessage`.
/// </summary>
public class ErrorCode : IEquatable<ErrorCode>, IComparable<ErrorCode>
{
    public string Value { get; }

    public ErrorCode(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value ?? string.Empty;
    }

    // Implicit conversion: Allows assigning a string to an ErrorCode directly.
    // Example: string code = new ErrorCode("UserDoesNotExist").
    public static implicit operator string(ErrorCode errorCode)
    {
        return errorCode.Value;
    }

    // Explicit conversion: Requires a cast to convert ErrorCode to string.
    // Example: string code = "UserDoesNotExist"; ErrorCode errorCode = (ErrorCode)code.
    public static explicit operator ErrorCode(string code)
    {
        return new ErrorCode(code);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ErrorCode);
    }

    public bool Equals(ErrorCode? other)
    {
        if (other is null) return false;

        // Strict type equality to prevent equality comparisons between a base class and its derived classes.
        if (GetType() != other.GetType()) return false;

        if (string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public static bool operator ==(ErrorCode a, ErrorCode b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ErrorCode a, ErrorCode b)
    {
        return !(a == b);
    }

    public int CompareTo(ErrorCode? other)
    {
        // Null is always considered "less than" any instance.
        if (other is null) return 1;

        // Performance optimization: Avoid redundant comparison when comparing the same object instance.
        if (ReferenceEquals(this, other)) return 0;

        // Handle null Code values explicitly
        if (Value is null && other.Value is null) return 0; // Both null -> equal
        if (Value is null) return -1; // Null is considered "less than" any non-null value
        if (other.Value is null) return 1; // Non-null is "greater than" null

        return string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }
}
