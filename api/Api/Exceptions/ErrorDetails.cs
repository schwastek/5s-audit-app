﻿using Core.MappingService;
using System.Text.Json;

namespace Api.Exceptions;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ErrorDetails(int statusCode)
    {
        StatusCode = statusCode;
        Message = GetDefaultMessageForStatusCode(statusCode);
    }

    public ErrorDetails(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request",
            401 => "Unauthorized",
            404 => "Not found",
            500 => "Internal server error",
            _ => string.Empty
        };
    }
}

public class ErrorDetailsMapper :
    IMapper<ErrorDetails, Contracts.Common.ErrorDetails>
{
    public Contracts.Common.ErrorDetails Map(ErrorDetails src)
    {
        return new Contracts.Common.ErrorDetails()
        {
            StatusCode = src.StatusCode,
            Message = src.Message
        };
    }
}
