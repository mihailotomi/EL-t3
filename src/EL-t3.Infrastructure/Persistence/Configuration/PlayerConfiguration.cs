using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("players");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(p => p.FirstName)
               .HasColumnName("first_name")
               .IsRequired();

        builder.Property(p => p.LastName)
               .HasColumnName("last_name")
               .IsRequired();

        builder.Property(p => p.BirthDate)
               .HasColumnName("birth_date")
               .IsRequired();

        builder.Property(p => p.Country)
               .HasColumnName("country")
               .HasMaxLength(3);

        builder.Property(p => p.ImageUrl)
               .HasColumnName("image_url");

        builder.Property(p => p.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(p => new { p.FirstName, p.LastName, p.BirthDate })
               .IsUnique();

        builder.HasIndex(p => new { p.FirstName, p.LastName });
    }
}

