using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasIndex(x => x.VerificationId).IsUnique();
        builder.Property(x => x.VerificationId).HasMaxLength(100);
        builder.Property(x => x.PdfReference).HasMaxLength(500);
    }
}
