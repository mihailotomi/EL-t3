using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public interface IPlayerBySeasonGateway
{
    Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<string> errors)> FetchPlayerSeasonsBySeasonAsync(int season);
}