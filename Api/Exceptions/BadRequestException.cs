using System;

namespace Api.Exceptions;

public abstract class BadRequestException : Exception
{
    protected BadRequestException(string message)
    : base(message)
    {
    }
}
