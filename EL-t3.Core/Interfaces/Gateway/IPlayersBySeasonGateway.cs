using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public record GatewayPlayerFailure(string PlayerName, IEnumerable<string> Errors);

public interface IPlayerBySeasonGateway
{
    Task<(IEnumerable<PlayerSeason> playerSeasons, IEnumerable<GatewayPlayerFailure> errors)> FetchPlayerSeasonsBySeasonAsync(int season);
}