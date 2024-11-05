using EL_t3.Application.Common.Interfaces.Repository;
using EL_t3.Domain.Entities;

namespace EL_t3.Infrastructure.Database.Repository;

public class PlayerSeasonRepository : BaseRepository<PlayerSeason>, IPlayerSeasonRepository
{
    public PlayerSeasonRepository(AppDatabaseContext context) : base(context)
    { }
}