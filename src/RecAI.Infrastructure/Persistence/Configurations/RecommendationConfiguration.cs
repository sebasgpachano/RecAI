using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecAI.Domain.Entities;

namespace RecAI.Infrastructure.Persistence.Configurations;

public class RecommendationConfiguration : IEntityTypeConfiguration<Recommendation>
{
    public void Configure(EntityTypeBuilder<Recommendation> builder)
    {
        builder.ToTable("recommendations");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Description).IsRequired().HasMaxLength(2000);

        // Store enums as readable strings ("High", "Pending") instead of ints.
        builder.Property(r => r.Priority)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(r => r.Status)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(r => r.CreatedAt).IsRequired();
    }
}