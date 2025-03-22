using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable("clubs", AppDatabaseContext.SchemaName);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
               .IsRequired();

        builder.Property(c => c.Code)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(c => c.CrestUrl);

        builder.Property(c => c.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(c => c.Code)
               .IsUnique();

        builder.HasIndex(c => c.CrestUrl)
               .IsUnique();
    }
}

