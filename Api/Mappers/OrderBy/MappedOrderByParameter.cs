namespace Api.Mappers.OrderBy;

public record MappedOrderByParameter
{
    public string PropertyName { get; init; } = string.Empty;

    // If request has `Age`, but model has `DateOfBirth` property, order must be reversed.
    // `Age` ascending (20, 21...) = `DateOfBirth` descending (2001, 2000...).
    public bool Reverse { get; init; }
}
