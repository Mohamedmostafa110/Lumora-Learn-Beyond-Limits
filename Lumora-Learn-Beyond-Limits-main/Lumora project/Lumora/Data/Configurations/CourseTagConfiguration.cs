using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class CourseTagConfiguration : IEntityTypeConfiguration<CourseTag>
{
    public void Configure(EntityTypeBuilder<CourseTag> builder)
    {
        builder.ToTable("Course_Tag");
        builder.HasKey(x => new { x.CourseId, x.TagId });

        builder.HasOne(x => x.Course).WithMany(c => c.CourseTags)
            .HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Tag).WithMany(t => t.CourseTags)
            .HasForeignKey(x => x.TagId);
    }
}
