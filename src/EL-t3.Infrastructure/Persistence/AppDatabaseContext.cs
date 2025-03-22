using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Infrastructure.Persistence;

public class AppDatabaseContext : DbContext, IAppDatabaseContext
{
    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
    {
    }

    public static string SchemaName {get; set;} = "euroleague-grids";

    public DbSet<Club> Clubs { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerSeason> PlayerSeasons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}