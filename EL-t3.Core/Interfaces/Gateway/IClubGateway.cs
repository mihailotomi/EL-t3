using EL_t3.Core.Entities;

namespace EL_t3.Core.Interfaces.Gateway;

public interface IClubGateway
{
    Task<(IEnumerable<Club> clubs, IEnumerable<string> errors)> FetchClubsBySeasonAsync(int season);
}