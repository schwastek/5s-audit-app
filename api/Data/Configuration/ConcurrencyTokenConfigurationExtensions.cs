using Domain.Concurrency;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public static class ConcurrencyTokenConfigurationExtensions
{
    public static void ConfigureConcurrencyToken<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IConcurrencyToken
    {
        builder.Property(e => e.Version)
            .IsConcurrencyToken()
            // We control increment.
            .ValueGeneratedNever();
    }
}
