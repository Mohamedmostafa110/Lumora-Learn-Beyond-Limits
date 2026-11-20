using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
{
    public void Configure(EntityTypeBuilder<CourseCategory> builder)
    {
        builder.ToTable("Course_Category");
        builder.HasKey(x => new { x.CourseId, x.CategoryId });

        builder.HasOne(x => x.Course).WithMany(c => c.CourseCategories)
            .HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Category).WithMany(c => c.CourseCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}
