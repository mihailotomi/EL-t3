using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Common.Interfaces.Context;

public interface IAppDatabaseContext
{
    public DbSet<Domain.Entities.Player> Players { get; set; }
    public DbSet<EL_t3.Domain.Entities.Club> Clubs { get; set; }
    public DbSet<EL_t3.Domain.Entities.PlayerSeason> PlayerSeasons { get; set; }

    public DbSet<T> Set<T>() where T : class;
}
