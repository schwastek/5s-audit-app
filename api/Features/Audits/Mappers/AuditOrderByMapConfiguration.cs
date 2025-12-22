using Domain;
using Features.Audits.Dto;
using Infrastructure.OrderBy;
using System;
using System.Collections.Generic;

namespace Features.Audits.Mappers;

public sealed class AuditOrderByMapConfiguration : IOrderByMap<Audit>
{
    // You can create one to many mapping, for example:
    // - "FullName" (request) -> "FirstName" & "LastName" (DB),
    // - "Age ASC" (request) -> "DateOfBirth DESC" (DB).
    private static readonly Dictionary<string, OrderByTarget[]> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(AuditDto.Author)] =
            [
                new() { PropertyName = nameof(Audit.Author) }
            ],
            [nameof(AuditDto.Area)] =
            [
                new() { PropertyName = nameof(Audit.Area) }
            ]
        };

    public IReadOnlyList<OrderByTarget>? GetMappingFor(string propertyName)
    {
        return _map.GetValueOrDefault(propertyName);
    }
}
