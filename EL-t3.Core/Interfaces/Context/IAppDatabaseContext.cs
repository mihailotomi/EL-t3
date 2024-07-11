using EL_t3.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Core.Interfaces.Context;

public interface IAppDatabaseContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<PlayerSeason> PlayerSeasons { get; set; }

    public DbSet<T> Set<T>() where T : class;
}
