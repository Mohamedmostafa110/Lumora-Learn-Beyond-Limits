using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class ReviewVoteConfiguration : IEntityTypeConfiguration<ReviewVote>
{
    public void Configure(EntityTypeBuilder<ReviewVote> builder)
    {
        builder.HasIndex(x => new { x.StudentId, x.ReviewId }).IsUnique();

        builder.HasOne(x => x.Review).WithMany(r => r.Votes)
            .HasForeignKey(x => x.ReviewId).OnDelete(DeleteBehavior.Cascade);
    }
}
