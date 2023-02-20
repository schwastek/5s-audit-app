using Api.Core.Domain;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services;

public class PropertyMappingService : IPropertyMappingService
{
    // Container for all mappings
    private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

    // You can create one to many mapping, e.g. "Name" (request) -> "FirstName" & "LastName" (model)
    private readonly Dictionary<string, PropertyMappingValue> _auditPropertyMapping =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Author", new PropertyMappingValue(new List<string>() { "Author" }) }
        };

    public PropertyMappingService()
    {
        _propertyMappings.Add(new PropertyMapping<AuditDto, Audit>(_auditPropertyMapping));
    }

    public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
    {
        if (string.IsNullOrWhiteSpace(fields)) return true;

        Dictionary<string, PropertyMappingValue> propertyMapping = GetPropertyMapping<TSource, TDestination>();

        // "author asc, created desc" -> ["author asc", "created desc"]
        string[] fieldsAfterSplit = fields.Split(',');

        foreach (string field in fieldsAfterSplit)
        {
            string trimmedField = field.Trim();

            // "created desc" -> "created"
            int indexOfFirstSpace = trimmedField.IndexOf(" ");
            string propertyName = indexOfFirstSpace == -1 ?
                trimmedField : trimmedField.Remove(indexOfFirstSpace);

            // Check if the property exists (if user can order data by the field)
            if (!propertyMapping.ContainsKey(propertyName))
            {
                return false;
            }
        }

        return true;
    }

    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        // Search for elements by their types, e.g. mapping between properties
        // of "AuditDto" (from request) and "Audit" (DB entity).
        IEnumerable<PropertyMapping<TSource, TDestination>> matchingMapping = _propertyMappings
            .OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Any())
        {
            return matchingMapping.First()._mappingDictionary;
        }

        throw new Exception($"Cannot find property mapping " +
            $"between {typeof(TSource)} and {typeof(TDestination)}");
    }
}
