using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class PlayerSeasonConfiguration : IEntityTypeConfiguration<PlayerSeason>
{
    public void Configure(EntityTypeBuilder<PlayerSeason> builder)
    {
        builder.ToTable("player_seasons");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Id)
               .HasColumnName("id")
               .ValueGeneratedOnAdd();

        builder.Property(ps => ps.ClubId)
               .HasColumnName("club_id");

        builder.Property(ps => ps.PlayerId)
               .HasColumnName("player_id");

        builder.Property(ps => ps.Season)
               .HasColumnName("season")
               .IsRequired();

        builder.Property(ps => ps.StartDate)
               .HasColumnName("start_date")
               .IsRequired();

        builder.Property(ps => ps.EndDate)
               .HasColumnName("end_date")
               .IsRequired();

        builder.Property(ps => ps.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(ps => ps.UpdatedAt)
               .HasColumnName("updated_at")
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

