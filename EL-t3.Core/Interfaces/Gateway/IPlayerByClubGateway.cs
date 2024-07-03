using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public interface IPlayerByClubGateway
{
    Task<IEnumerable<Player>> FetchPlayersByClubAsync(string clubCode);
    Task<IEnumerable<PlayerSeason>> FetchPlayerSeasonsByClubAsync(string clubCode);
}