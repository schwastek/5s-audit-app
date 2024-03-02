using System;

namespace Api.Mappers.MappingService;

public sealed class MapperNotFoundException : Exception
{
    public MapperNotFoundException(Type source, Type destination)
        : base($"No Mapper from '{source}' to '{destination}' was found.")
    {
    }
}
