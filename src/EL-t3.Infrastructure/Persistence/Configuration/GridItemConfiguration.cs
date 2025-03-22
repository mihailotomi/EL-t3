using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EL_t3.Infrastructure.Persistence.Configuration;

public class GridItemConfiguration : IEntityTypeConfiguration<GridItem>
{
    public void Configure(EntityTypeBuilder<GridItem> builder)
    {
        builder.ToTable("grid_items", AppDatabaseContext.SchemaName);

        builder.HasKey(p => p.Id);

        builder.HasDiscriminator<string>("GridItemType")
            .HasValue<ClubGridItem>("Club")
            .HasValue<CountryGridItem>("Country")
            .HasValue<TeammateGridItem>("Teammate");
    }
}

public class ClubGridItemConfiguration : IEntityTypeConfiguration<ClubGridItem>
{
    public void Configure(EntityTypeBuilder<ClubGridItem> builder)
    {
        builder.HasOne(gi => gi.Club)
             .WithMany()
             .HasForeignKey(gi => gi.ClubId)
             .OnDelete(DeleteBehavior.ClientCascade);
    }
}

public class CountryGridItemConfiguration : IEntityTypeConfiguration<CountryGridItem>
{
    public void Configure(EntityTypeBuilder<CountryGridItem> builder)
    {
        builder.Property(gi => gi.Country)
            .HasMaxLength(3);
    }
}

public class TeammateGridItemConfiguration : IEntityTypeConfiguration<TeammateGridItem>
{
    public void Configure(EntityTypeBuilder<TeammateGridItem> builder)
    {
        builder.HasOne(gi => gi.Teammate)
             .WithMany()
             .HasForeignKey(gi => gi.TeammateId)
             .OnDelete(DeleteBehavior.ClientCascade);
    }
}