using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class MediaAssetConfiguration : IEntityTypeConfiguration<MediaAsset>
{
    public void Configure(EntityTypeBuilder<MediaAsset> builder)
    {
        builder.Property(x => x.StorageKey).HasMaxLength(500);
        builder.Property(x => x.ThumbnailKey).HasMaxLength(500);
        builder.Property(x => x.ProcessingStatus).HasConversion<string>().HasMaxLength(30);

        builder.HasOne(x => x.Lesson).WithMany(l => l.MediaAssets)
            .HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Cascade);
    }
}
