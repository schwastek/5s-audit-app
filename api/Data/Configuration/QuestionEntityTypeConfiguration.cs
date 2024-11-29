using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class QuestionEntityTypeConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable(Tables.Questions);

        builder.HasKey(q => q.QuestionId);

        builder.Property(q => q.QuestionText)
            .IsRequired();
    }
}
