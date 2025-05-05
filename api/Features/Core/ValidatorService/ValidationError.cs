using Domain.Exceptions;

namespace Features.Core.ValidatorService;

/// <summary>
/// Represents a validation error for a specific property in a request or command.
/// </summary>
public record ValidationError
{
    /// <summary>
    /// A machine-readable error code identifying the specific validation failure.
    /// Error codes should be entity-specific and unique to avoid confusion.
    /// Do not reuse generic error codes like `IdIsRequired` across different entities.
    /// Using specific error codes makes debugging and client-side error handling much easier.
    /// Example: `AuditIdIsRequired`, `AuditActionIdIsRequired`.
    /// </summary>
    public string ErrorCode { get; private set; }

    public ValidationError(ErrorCode errorCode)
    {
        ErrorCode = errorCode;
    }

    public override string ToString()
    {
        return ErrorCode;
    }
}
