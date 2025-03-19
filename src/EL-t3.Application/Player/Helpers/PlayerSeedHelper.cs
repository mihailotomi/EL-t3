using EL_t3.Application.Common.Interfaces.Context;
using EL_t3.Application.Player.Payloads;
using EL_t3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EL_t3.Application.Player.Helpers;

public class PlayerSeedHelper
{
    private readonly IServiceProvider _serviceProvider;

    public PlayerSeedHelper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Extracts all unique players in a CreatePlayerSeasonPayload enumerable and prepares Players for persistence.
    /// </summary>
    /// <param name="playerSeasons"></param>
    /// <returns>A list of players</returns>
    public static List<Domain.Entities.Player> PrepeareUniquePlayersFromPlayerSeasons(IEnumerable<CreatePlayerSeasonPayload> playerSeasons)
    {
        return playerSeasons
            .Select(rp => Domain.Entities.Player.Create(rp.FirstName, rp.LastName, rp.BirthDate, rp.Country, rp.ImageUrl))
            .Where(p => p != null)
            .GroupBy(p => new { p!.FirstName, p!.LastName, p!.BirthDate })
            .Select(g => g.First()!)
            .ToList();
    }

    /// <summary>
    /// Adds the club and player id to a CreatePlayerSeasonPayload, while removing duplicates and preparing domain entities for persistence.
    /// </summary>
    /// <param name="rawPlayerSeasons">An enumerable of player season paayloads, without club and player ids, typically obtained directly from data source.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of proccessed player seasons</returns>
    public async Task<IList<PlayerSeason>> PreparePlayerSeasons(IEnumerable<CreatePlayerSeasonPayload> rawPlayerSeasons, CancellationToken cancellationToken)
    {
        var playerSeasons = new List<PlayerSeason>();
        var semaphore = new SemaphoreSlim(10);

        var tasks = rawPlayerSeasons.Select(
            async ps =>
        {
            await semaphore.WaitAsync(cancellationToken);

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDatabaseContext>();

            var club = await dbContext.Clubs.Where(c => c.Code == ps.ClubCode).FirstOrDefaultAsync(cancellationToken);
            var player = await dbContext.Players
                .Where(p =>
                    p.FirstName == ps.FirstName &&
                    p.LastName == ps.LastName &&
                    p.BirthDate == ps.BirthDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (player is null || club is null)
            {
                // TODO - Append to log file - player not found
                return;
            }

            try
            {
                var playerSeason = PlayerSeason.Create
                (
                    playerId: player.Id,
                    clubId: club.Id,
                    season: ps.Season,
                    startDate: ps.StartedAt,
                    endDate: ps.EndedAt
                );

                playerSeasons.Add(playerSeason);
            }
            catch
            {
                // TODO - Append to log file - player not found
                return;
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
        return playerSeasons;
    }
}
