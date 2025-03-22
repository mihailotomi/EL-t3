using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class PlayerSeasonConfiguration : IEntityTypeConfiguration<PlayerSeason>
{
    public void Configure(EntityTypeBuilder<PlayerSeason> builder)
    {
        builder.ToTable("player_seasons", AppDatabaseContext.SchemaName);

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
               .ValueGeneratedOnAdd();

        builder.Property(ps => ps.Season)
               .IsRequired();

        builder.Property(ps => ps.StartDate)
               .IsRequired();

        builder.Property(ps => ps.EndDate)
               .IsRequired();

        builder.Property(ps => ps.CreatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(ps => ps.UpdatedAt)
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(ps => new { ps.ClubId, ps.PlayerId, ps.Season })
               .IsUnique();

        builder.HasOne(ps => ps.Club)
               .WithMany()
               .HasForeignKey(ps => ps.ClubId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ps => ps.Player)
               .WithMany(p => p.SeasonsPlayed)
               .HasForeignKey(ps => ps.PlayerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

