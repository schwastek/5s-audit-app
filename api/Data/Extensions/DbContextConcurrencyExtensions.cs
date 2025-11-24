using Data.DbContext;
using Domain.Concurrency;
using Microsoft.EntityFrameworkCore;

namespace Data.Extensions;

public static class DbContextConcurrencyExtensions
{
    /// <summary>
    /// Applies the client's concurrency version to the entity 
    /// so Entity Framework can detect optimistic concurrency conflicts.
    /// EF compares this original value with the current value in the database during SaveChanges.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="context">DbContext instance</param>
    /// <param name="entity">Tracked entity</param>
    /// <param name="clientVersion">Last known version supplied by the client</param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void SetConcurrencyToken<TEntity>(
        this LeanAuditorContext context,
        TEntity entity,
        long clientVersion)
        where TEntity : class, IConcurrencyToken
    {
        var entry = context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            throw new InvalidOperationException("Entity must be tracked by DbContext.");
        }

        entry.Property(e => e.Version).OriginalValue = clientVersion;
    }
}
