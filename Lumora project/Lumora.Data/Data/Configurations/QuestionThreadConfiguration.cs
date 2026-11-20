using Lumora.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumora.Data.Configurations;

public class QuestionThreadConfiguration : IEntityTypeConfiguration<QuestionThread>
{
    public void Configure(EntityTypeBuilder<QuestionThread> builder)
    {
        // Keys, FKs and navigations are covered by EF conventions; add rules here as needed.
    }
}
