using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public interface IPlayerBySeasonGateway
{
    Task<IEnumerable<Player>> FetchPlayersBySeasonAsync(int season);
    Task<IEnumerable<PlayerSeason>> FetchPlayerSeasonsBySeasonAsync(int seasons);
}