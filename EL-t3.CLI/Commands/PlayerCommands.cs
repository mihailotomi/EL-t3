using System.Text.Json;
using Cocona;
using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Gateway;
using EL_t3.Core.Interfaces.Repository;

namespace EL_t3.CLI.Commands;

public class PlayerCommands
{
    private readonly IPlayerBySeasonGateway _playerBySeasonGateway;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerSeasonRepository _playerSeasonRepository;
    private readonly IClubRepository _clubRepository;



    public PlayerCommands(IPlayerBySeasonGateway playerBySeasonGateway, IPlayerRepository playerRepository, IPlayerSeasonRepository playerSeasonRepository, IClubRepository clubRepository)
    {
        _playerBySeasonGateway = playerBySeasonGateway;
        _playerRepository = playerRepository;
        _playerSeasonRepository = playerSeasonRepository;
        _clubRepository = clubRepository;
    }

    [Command("seed-players", Description = "Seeds players and player-seasons for all seasons from 2000 onward")]
    public async Task SeedPlayers()
    {
        for (int season = 2000; season <= 2023; season++)
        {
            Console.WriteLine($"Seeding players for season {season}");
            var (rawPlayerSeasons, gatewayErrors) = await _playerBySeasonGateway.FetchPlayerSeasonsBySeasonAsync(season);
            if (gatewayErrors.Any())
            {
                // Append to log file - player not properly fetched
                Console.Error.WriteLine($"There are errors for season {season}");
            }

            try
            {
                var players = rawPlayerSeasons.Select(ps => ps.Player)
                    .Where(p => p != null)
                    .GroupBy(p => new { p!.FirstName, p.LastName, p.BirthDate })
                    .Select(g => g.First())
                    .ToList();

                await _playerRepository.UpsertManyAsync(players!, (p) => new { p.FirstName, p.LastName, p.BirthDate });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }


            var playerSeasons = await PreparePlayerSeasons(rawPlayerSeasons);
            try
            {
                await _playerSeasonRepository.UpsertManyAsync(playerSeasons, (p) => new { p.PlayerId, p.ClubId, p.Season });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }


            Console.WriteLine($"Finished seeding players for season {season}");
        }
    }

    private async Task<IList<PlayerSeason>> PreparePlayerSeasons(IEnumerable<PlayerSeason> rawPlayerSeasons)
    {
        var playerSeasons = new List<PlayerSeason>();
        var foundPlayerIds = new HashSet<int>();

        foreach (var ps in rawPlayerSeasons)
        {
            var club = await _clubRepository.FindOneAsync(c => c.Code == ps.Club!.Code);
            var player = await _playerRepository.FindOneAsync(p =>
                p.FirstName == ps.Player!.FirstName &&
                p.LastName == ps.Player.LastName &&
                p.BirthDate == ps.Player.BirthDate);

            if (player == null || club == null)
            {
                // Append to log file - player not found
                continue;
            }
            if (foundPlayerIds.Contains(player.Id))
            {
                // Append to log file - duplicate player for season
                continue;
            }

            ps.ClubId = club!.Id;
            ps.PlayerId = player!.Id;

            playerSeasons.Add(ps);
            foundPlayerIds.Add(player.Id);

        }

        return playerSeasons;
    }

}
