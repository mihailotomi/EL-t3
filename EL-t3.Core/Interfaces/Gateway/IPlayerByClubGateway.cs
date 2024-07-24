using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public interface IPlayerByClubGateway
{
    Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<string> errors)> FetchPlayersSeasonsByClubAsync(string clubCode);
}