using EL_t3.Domain.Entities;

namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IPlayerBySeasonGateway
{
    Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<string> errors)> FetchPlayerSeasonsBySeasonAsync(int season);
}