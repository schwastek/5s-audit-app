using Domain.Auditing;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public static class AuditableEntityConfigurationExtensions
{
    public static void ConfigureAuditableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IAuditableEntity
    {
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ModifiedAt)
            .IsRequired();
    }
}
