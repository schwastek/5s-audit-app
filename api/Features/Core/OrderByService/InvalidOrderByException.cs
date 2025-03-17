using Features.Core.Exceptions;
using Domain.Exceptions;

namespace Features.Core.OrderByService;

public sealed class InvalidOrderByException : ApplicationValidationException
{
    public InvalidOrderByException(string propertyQuery) :
        base($"The specified field '{propertyQuery}' is not valid for sorting.", ErrorCodes.Audit.InvalidOrderByField)
    {
    }
}
