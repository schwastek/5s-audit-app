namespace Domain.Concurrency;

/// <summary>
/// Defines an entity that exposes a concurrency token
/// used by Entity Framework to detect conflicting updates.
/// </summary>
public interface IConcurrencyToken
{
    /// <summary>
    /// Incremental version used for optimistic concurrency check.
    /// </summary>
    long Version { get; }
}
