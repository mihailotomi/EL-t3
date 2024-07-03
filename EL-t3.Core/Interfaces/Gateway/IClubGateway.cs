using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public record GatewayClubFailure(string ClubCode, IEnumerable<string> Errors);

public interface IClubGateway
{
    Task<(IEnumerable<Club> clubs, IEnumerable<GatewayClubFailure> errors)> FetchClubsBySeasonAsync(int season);
}