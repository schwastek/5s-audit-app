using Core.Exceptions;

namespace Core.OrderByService;

public sealed class InvalidOrderByException : BadRequestException
{
    public InvalidOrderByException(string propertyQuery) :
        base($"The specified field '{propertyQuery}' is not valid for sorting.")
    {
    }
}
