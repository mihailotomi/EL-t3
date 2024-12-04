using EL_t3.Application.Player.Payloads;
using EL_t3.Infrastructure.Gateway.Contracts;

namespace EL_t3.Infrastructure.Gateway.Extensions;

public static class GatewayPlayerSeasonMappingExtensions
{
    public static CreatePlayerSeasonPayload ToPayload(this GatewayPlayerSeason ps)
    {
        var nameParts = ps.Person.Name.Split(',');

        return new CreatePlayerSeasonPayload
        (
            FirstName: nameParts![1].ToUpper().Trim(),
            LastName: nameParts![0].ToUpper().Trim(),
            ImageUrl: ps.Images?.Headshot,
            BirthDate: DateOnly.Parse(ps.Person.BirthDate),
            Country: ps.Person.Country.Code,
            Season: ps.Season.Year,
            ClubCode: ps.Club.Code,
            StartedAt: DateOnly.Parse(ps.StartDate),
            EndedAt: DateOnly.Parse(ps.EndDate)

        );
    }
}