using System;

namespace Domain.Auditing;

/// <summary>
/// Defines audit properties required for persistence.
/// These values are set by infrastructure (e.g. EF SaveChanges interceptor) 
/// and should not be modified directly by domain or application logic.
/// </summary>
/// <remarks>
/// Audit properties belong to the persistence model, not the domain model.
/// However, this pattern is required when the domain entity and EF model are merged
/// into a single class for convenience. This is a tradeoff of the "one class for both domain + EF" design.
/// </remarks>
public interface IAuditableEntity
{
    /// <summary>
    /// The user who created the entity.
    /// Set once by infrastructure when the entity is first persisted.
    /// </summary>
    string CreatedBy { get; }

    /// <summary>
    /// The timestamp when the entity was created.
    /// Set once by infrastructure when the entity is first persisted.
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// The user who last modified the entity.
    /// Updated by infrastructure on every save.
    /// </summary>
    string ModifiedBy { get; }

    /// <summary>
    /// The timestamp when the entity was last modified.
    /// Updated by infrastructure on every save.
    /// </summary>
    DateTimeOffset ModifiedAt { get; }
}
