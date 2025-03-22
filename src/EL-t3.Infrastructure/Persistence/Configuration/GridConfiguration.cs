using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GridConfiguration : IEntityTypeConfiguration<Grid>
{
    public void Configure(EntityTypeBuilder<Grid> builder){
        builder.ToTable("grids", AppDatabaseContext.SchemaName);

        builder.HasKey(p => p.Id);

        builder
            .HasMany(g => g.X)
            .WithMany()
            .UsingEntity(
                "grid-members-x",
                j => j.HasOne(typeof(GridItem)).WithMany().HasForeignKey("grid_item_id"),
                j => j.HasOne(typeof(Grid)).WithMany().HasForeignKey("grid_id"),
                j => j.HasKey("grid_item_id", "grid_id"));

        builder
            .HasMany(g => g.Y)
            .WithMany()
            .UsingEntity(
                "grid-members-y",
                j => j.HasOne(typeof(GridItem)).WithMany().HasForeignKey("grid_item_id"),
                j => j.HasOne(typeof(Grid)).WithMany().HasForeignKey("grid_id"),
                j => j.HasKey("grid_item_id", "grid_id"));
    }
}