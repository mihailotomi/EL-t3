using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Player.Payloads;
using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Application.Player.Helpers;

public class PlayerSeedHelper
{
    private readonly IAppDatabaseContext _dbContext;

    public PlayerSeedHelper(IAppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Extracts all unique players in a PlayerSeason enumerable.
    /// </summary>
    /// <param name="playerSeasons"></param>
    /// <returns>A list of players</returns>
    public static List<Domain.Entities.Player> ExtractUniquePlayersFromPlayerSeasons(IEnumerable<CreatePlayerSeasonPayload> playerSeasons)
    {
        return playerSeasons
            .Select(ps => new Domain.Entities.Player() { FirstName = ps.FirstName, LastName = ps.LastName, BirthDate = ps.BirthDate, Country = ps.Country, ImageUrl = ps.ImageUrl })
            .Where(p => p != null)
            .GroupBy(p => new { p!.FirstName, p!.LastName, p!.BirthDate })
            .Select(g => g.First()!)
            .ToList();
    }

    /// <summary>
    /// Adds the club and player id to a PlayerSeason without it, while removing duplicates.
    /// </summary>
    /// <param name="rawPlayerSeasons">An enumerable of player seasons, without club and player ids, typically obtained directly from data source.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of proccessed player seasons</returns>
    public async Task<IList<PlayerSeason>> PreparePlayerSeasons(IEnumerable<CreatePlayerSeasonPayload> rawPlayerSeasons, CancellationToken cancellationToken)
    {
        var playerSeasons = new List<PlayerSeason>();

        foreach (var ps in rawPlayerSeasons)
        {
            var club = await _dbContext.Clubs.Where(c => c.Code == ps.ClubCode).FirstOrDefaultAsync(cancellationToken);
            var player = await _dbContext.Players
                .Where(p =>
                    p.FirstName == ps.FirstName &&
                    p.LastName == ps.LastName &&
                    p.BirthDate == ps.BirthDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (player == null || club == null)
            {
                // Append to log file - player not found
                continue;
            }

            var playerSeason = new PlayerSeason()
            {
                PlayerId = player.Id,
                ClubId = club.Id,
                Season = ps.Season,
                StartDate = ps.StartedAt,
                EndDate = ps.EndedAt,
            };

            playerSeasons.Add(playerSeason);
        }

        return playerSeasons;
    }
}
