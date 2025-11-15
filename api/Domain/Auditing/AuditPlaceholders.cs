namespace Domain.Auditing;

/// <summary>
/// Placeholder values for audit properties in domain entities. 
/// Domain sets them to avoid nulls, infrastructure overwrites with real user/timestamp before saving.
/// </summary>
/// <remarks>
/// <para>
/// Audit fields like <see cref="IAuditableEntity.CreatedBy"/> and <see cref="IAuditableEntity.ModifiedBy"/> are non-nullable in the database
/// and are normally populated by infrastructure (e.g. EF SaveChanges interceptor).
/// The domain cannot know the real user when creating an entity.
/// </para>
/// <para>
/// Placeholders guarantee that domain entities are always valid objects (no nulls)
/// while allowing infrastructure to overwrite them with actual audit values before saving.
/// These placeholder values never reach the database.
/// </para>
/// </remarks>
public static class AuditPlaceholders
{
    public const string Unknown = "Unknown";
}
