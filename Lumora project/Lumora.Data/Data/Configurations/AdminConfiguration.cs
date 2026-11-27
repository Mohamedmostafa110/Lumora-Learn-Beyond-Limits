using Lumora.Data.Identity;
using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.UserId).HasMaxLength(450);
        builder.HasIndex(x => x.UserId).IsUnique().HasFilter("[UserId] IS NOT NULL");

        builder.HasOne<ApplicationUser>()
            .WithOne(u => u.AdminProfile)
            .HasForeignKey<Admin>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.FullName).HasMaxLength(200);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.AccountStatus).HasConversion<string>().HasMaxLength(30);
    }
}
