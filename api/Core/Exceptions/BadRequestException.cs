using System;

namespace Core.Exceptions;

public class BadRequestException : Exception
{
    protected BadRequestException(string message)
    : base(message)
    {
    }
}
