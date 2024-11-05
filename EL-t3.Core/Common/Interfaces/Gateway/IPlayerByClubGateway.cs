using EL_t3.Domain.Entities;

namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IPlayerByClubGateway
{
    Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<string> errors)> FetchPlayersSeasonsByClubAsync(string clubCode);
}