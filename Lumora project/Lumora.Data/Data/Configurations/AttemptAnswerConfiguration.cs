using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
{
    public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
    {
        builder.HasOne(x => x.QuizAttempt).WithMany(a => a.Answers)
            .HasForeignKey(x => x.QuizAttemptId).OnDelete(DeleteBehavior.Cascade);
    }
}
