using System.Collections.Generic;

namespace Api.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }

        // If request model has `Age` but DB entity has `DateOfBirth` property,
        // order must be reverted. `Age` ascending (20, 21...) = `DateOfBirth` descending (2001, 2000...)
        public bool Revert { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties;
            Revert = revert;
        }
    }
}