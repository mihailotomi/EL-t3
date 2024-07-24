using EL_t3.Core.Entities;
using EL_t3.Infrastructure.Gateway.Contracts;

namespace EL_t3.Infrastructure.Gateway.Extensions;

public static class GatewayPlayerSeasonMappingExtensions
{
    public static PlayerSeason MapToPlayerSeasonEntity(this GatewayPlayerSeason ps)
    {
        var nameParts = ps.Person.Name.Split(',');

        return new PlayerSeason
        {
            Player = new Player
            {
                LastName = nameParts![0].ToUpper().Trim(),
                FirstName = nameParts![1].ToUpper().Trim(),
                BirthDate = DateOnly.Parse(ps.Person.BirthDate),
                Country = ps.Person.Country.Code,
                ImageUrl = ps.Images?.Headshot,
            },
            Club = ps.Club.MapToClubEntity(),
            StartDate = DateOnly.Parse(ps.StartDate),
            EndDate = DateOnly.Parse(ps.EndDate),
            Season = ps.Season.Year,
        };
    }
}