using EL_t3.Application.Club.Payloads;

namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IClubBySeasonGateway
{
    Task<(IEnumerable<CreateClubPayload> payloads, IEnumerable<string> errors)> FetchClubsBySeasonAsync(int season);
}