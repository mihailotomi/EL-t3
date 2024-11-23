using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EL_t3.Application.Common.Interfaces.Context;

public interface IAppDatabaseContext
{
    public DbSet<Domain.Entities.Player> Players { get; set; }
    public DbSet<Domain.Entities.Club> Clubs { get; set; }
    public DbSet<Domain.Entities.PlayerSeason> PlayerSeasons { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();


    DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    ChangeTracker ChangeTracker { get; }
}
