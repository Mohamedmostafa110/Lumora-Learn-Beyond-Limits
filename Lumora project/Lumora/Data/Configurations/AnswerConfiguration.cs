using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasOne(x => x.QuestionThread).WithMany(t => t.Answers)
            .HasForeignKey(x => x.QuestionThreadId).OnDelete(DeleteBehavior.Cascade);
    }
}
