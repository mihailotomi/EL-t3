using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("players", AppDatabaseContext.SchemaName);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .ValueGeneratedOnAdd();

        builder.Property(p => p.FirstName)
               .IsRequired();

        builder.Property(p => p.LastName)
               .IsRequired();

        builder.Property(p => p.BirthDate)
               .IsRequired();

        builder.Property(p => p.Country)
               .HasMaxLength(3);

        builder.Property(p => p.ImageUrl);

        builder.Property(p => p.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(p => new { p.FirstName, p.LastName, p.BirthDate })
               .IsUnique();

        builder.HasIndex(p => new { p.FirstName, p.LastName });
    }
}

