using EL_t3.Application.Player.Payloads;

namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IPlayerByClubGateway
{
    Task<(IEnumerable<CreatePlayerSeasonPayload> playerSeasons, IEnumerable<string> errors)> FetchPlayersSeasonsByClubAsync(string clubCode, bool isNba = false, CancellationToken cancellationToken = default);
}