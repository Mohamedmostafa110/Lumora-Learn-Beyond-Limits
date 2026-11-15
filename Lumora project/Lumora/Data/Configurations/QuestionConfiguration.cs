using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);

        builder.HasOne(x => x.Quiz).WithMany(q => q.Questions)
            .HasForeignKey(x => x.QuizId).OnDelete(DeleteBehavior.Cascade);
    }
}
