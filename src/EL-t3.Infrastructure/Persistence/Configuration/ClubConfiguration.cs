using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable("clubs");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
               .HasColumnName("name")
               .IsRequired();

        builder.Property(c => c.Code)
               .HasColumnName("code")
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(c => c.CrestUrl)
               .HasColumnName("crest_url");

        builder.Property(c => c.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(c => c.Code)
               .IsUnique();

        builder.HasIndex(c => c.CrestUrl)
               .IsUnique();
    }
}

