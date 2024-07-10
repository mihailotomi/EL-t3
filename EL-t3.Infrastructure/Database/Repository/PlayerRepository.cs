using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Repository;

namespace EL_t3.Infrastructure.Database.Repository;

public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
{
    public PlayerRepository(AppDatabaseContext context) : base(context)
    { }
}