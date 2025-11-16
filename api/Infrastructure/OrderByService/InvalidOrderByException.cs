using Infrastructure.Exceptions;

namespace Infrastructure.OrderByService;

public sealed class InvalidOrderByException : ApplicationException
{
    public InvalidOrderByException(string propertyQuery) :
        base($"The specified field '{propertyQuery}' is not valid for sorting.")
    {
    }
}
