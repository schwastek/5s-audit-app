namespace Api.Exceptions;

public sealed class IncorrectPropertyBadRequestException : BadRequestException
{
    public IncorrectPropertyBadRequestException(string propertyQuery) : 
        base($"Cannot find property mapping for '{propertyQuery}'")
    {
    }
}
