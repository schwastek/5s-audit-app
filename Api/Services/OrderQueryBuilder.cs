using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Services;

public class OrderQueryBuilder
{
    public static string CreateOrderQuery<T>(string orderBy, Dictionary<string, PropertyMappingValue> mapping)
    {
        var orderParams = orderBy.Split(',');
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            var trimmed = param.Trim();

            // Get property name, "author desc" -> "author"
            int space = trimmed.IndexOf(" ");
            string propertyName = space == -1 ? trimmed : trimmed.Remove(space);

            if (!mapping.ContainsKey(propertyName))
            {
                throw new ArgumentException($"Key mapping for {propertyName} is missing");
            }

            PropertyMappingValue mappingValue = mapping[propertyName];

            if (mappingValue == null)
            {
                throw new ArgumentNullException($"Values missing for {propertyName}");
            }

            bool sortDescending = trimmed.EndsWith(" desc");

            // Revert sort order if necessary
            if (mappingValue.Revert)
            {
                sortDescending = !sortDescending;
            }

            string direction = sortDescending ? "descending" : "ascending";

            foreach (string destinationProperty in mappingValue.DestinationProperties)
            {
                orderQueryBuilder.Append($"{destinationProperty} {direction}, ");
            }

        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        return orderQuery;
    }
}
