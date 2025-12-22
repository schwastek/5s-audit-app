using Infrastructure.OrderBy;
using System;
using System.Collections.Generic;

namespace UnitTests.OrderBy.Service;

internal class FakeRequest
{
    public string Author { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
}

internal class FakeEntity
{
    public string Author { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

internal class FakeEntityOrderByMapConfiguration : IOrderByMap<FakeEntity>
{
    private static readonly Dictionary<string, OrderByTarget[]> _mappings =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(FakeRequest.Author)] =
            [
                new() { PropertyName = nameof(FakeEntity.Author) }
            ],

            [nameof(FakeRequest.Age)] =
            [
                new() { PropertyName = nameof(FakeEntity.DateOfBirth), ReverseDirection = true }
            ],

            [nameof(FakeRequest.FullName)] =
            [
                new() { PropertyName = nameof(FakeEntity.FirstName) },
                new() { PropertyName = nameof(FakeEntity.LastName) }
            ],
        };

    public IReadOnlyList<OrderByTarget>? GetMappingFor(string propertyName)
    {
        return _mappings.GetValueOrDefault(propertyName);
    }
}
