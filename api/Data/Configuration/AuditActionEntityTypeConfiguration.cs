using Domain;
using Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class AuditActionEntityTypeConfiguration : IEntityTypeConfiguration<AuditAction>
{
    public void Configure(EntityTypeBuilder<AuditAction> builder)
    {
        builder.ToTable(Tables.AuditActions);

        builder.HasKey(aa => aa.AuditActionId);

        // Without this configuration, EF Core may mistakenly treat the entity
        // as an update operation instead of an insert, leading to silent failures when trying to add the entity
        // to the database. This ensures EF Core correctly handles entities with pre-generated primary key values.
        // See: https://stackoverflow.com/questions/78510739/efcore-related-entity-not-saved
        // See: https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations#key-types-and-values
        builder.Property(aa => aa.AuditActionId).ValueGeneratedNever();

        builder.Property(aa => aa.Description)
            .IsRequired()
            .HasMaxLength(AuditActionConstants.DescriptionMaxLength);

        builder.Property(aa => aa.IsComplete)
            .IsRequired()
            .HasDefaultValue(false);

        builder.ConfigureAuditableProperties();
    }
}
