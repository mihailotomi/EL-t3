namespace EL_t3.Application.Common.Interfaces.Gateway;

public interface IClubGateway
{
    Task<(IEnumerable<Domain.Entities.Club> clubs, IEnumerable<string> errors)> FetchClubsBySeasonAsync(int season);
}