using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Repository;

namespace EL_t3.Infrastructure.Database.Repository;

public class PlayerSeasonRepository : BaseRepository<PlayerSeason>, IPlayerSeasonRepository
{
    public PlayerSeasonRepository(AppDatabaseContext context) : base(context)
    { }
}