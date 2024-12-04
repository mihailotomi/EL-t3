using EL_t3.Application.Player.Payloads;

namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IPlayerBySeasonGateway
{
    Task<(IEnumerable<CreatePlayerSeasonPayload> playerSeasons, IEnumerable<string> errors)> FetchPlayerSeasonsBySeasonAsync(int season);
}