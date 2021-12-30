using Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Api.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return source;

            // Final string to execute LINQ `.OrderBy()` query with.
            // This service converts "Author desc, Created asc" to "Author descending, Created ascending".
            string orderByString = string.Empty;

            // Split by "," (passed `orderBy` string example: "author, created desc")
            string[] orderByAfterSplit = orderBy.Split(',');
 
            foreach (string orderByClause in orderByAfterSplit)
            {
                string trimmedOrderByClause = orderByClause.Trim();

                // Remove " asc" or " desc" from the orderBy clause, 
                // "created desc" -> "created"
                int indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                string propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                // Check if the property exists 
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                // Get the `PropertyMappingValue`
                PropertyMappingValue propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException($"Values missing for {propertyName}");
                }

                // "created desc" -> we order descending, ortherwise ascending
                bool orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // Revert sort order if necessary
                if (propertyMappingValue.Revert)
                {
                    orderDescending = !orderDescending;
                }

                // Run through the property names, 
                // so the orderBy clauses are applied in the correct order
                foreach (string destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    orderByString = orderByString +
                        (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                        + destinationProperty
                        + (orderDescending ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);
        }
    }
}
