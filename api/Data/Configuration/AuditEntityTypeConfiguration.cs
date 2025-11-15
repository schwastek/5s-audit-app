using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class AuditEntityTypeConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable(Tables.Audits);

        builder.HasKey(a => a.AuditId);

        builder.Property(a => a.AuditId)
            .ValueGeneratedNever();

        builder.Property(a => a.Author)
            .IsRequired();

        builder.Property(a => a.Area)
            .IsRequired();

        builder.Property(a => a.StartDate)
            .IsRequired();

        builder.Property(a => a.EndDate)
            .IsRequired();

        builder.Ignore(a => a.Score);

        builder.HasMany(a => a.Actions)
            .WithOne()
            .HasForeignKey(act => act.AuditId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Answers)
            .WithOne()
            .HasForeignKey(ans => ans.AuditId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ConfigureAuditableProperties();
    }
}
