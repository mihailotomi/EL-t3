using Microsoft.EntityFrameworkCore;
using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Context;

namespace EL_t3.Infrastructure.Database;

public class AppDatabaseContext : DbContext, IAppDatabaseContext
{
    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
    {
    }

    public DbSet<Club> Clubs { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerSeason> PlayerSeasons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}