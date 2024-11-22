using EL_t3.Application.Common.Interfaces.Context;
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
    public static List<Domain.Entities.Player> ExtractUniquePlayersFromPlayerSeasons(IEnumerable<PlayerSeason> playerSeasons)
    {
        return playerSeasons
            .Select(ps => ps.Player)
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
    public async Task<IList<PlayerSeason>> PreparePlayerSeasons(IEnumerable<PlayerSeason> rawPlayerSeasons, CancellationToken cancellationToken)
    {
        var playerSeasons = new List<PlayerSeason>();

        foreach (var ps in rawPlayerSeasons)
        {
            var club = await _dbContext.Clubs.Where(c => c.Code == ps.Club!.Code).FirstOrDefaultAsync(cancellationToken);
            var player = await _dbContext.Players
                .Where(p =>
                    p.FirstName == ps.Player!.FirstName &&
                    p.LastName == ps.Player.LastName &&
                    p.BirthDate == ps.Player.BirthDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (player == null || club == null)
            {
                // Append to log file - player not found
                continue;
            }

            ps.ClubId = club!.Id;
            ps.PlayerId = player!.Id;

            playerSeasons.Add(ps);
        }

        return playerSeasons;
    }
}
