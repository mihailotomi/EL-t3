using System.Text.Json;
using Cocona;
using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Gateway.Helpers;
using System.Threading;
using System.Threading.Tasks;
using EL_t3.Application.Common.Interfaces.Gateway;
using EL_t3.Application.Common.Interfaces.Repository;

namespace EL_t3.CLI.Commands
{
    public class PlayerCommands
    {
        private readonly IPlayerBySeasonGateway _playerBySeasonGateway;
        private readonly IPlayerByClubGateway _playerByClubGateway;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerSeasonRepository _playerSeasonRepository;
        private readonly IClubRepository _clubRepository;

        public PlayerCommands(
            IPlayerBySeasonGateway playerBySeasonGateway,
            IPlayerRepository playerRepository,
            IPlayerSeasonRepository playerSeasonRepository,
            IClubRepository clubRepository,
            IPlayerByClubGateway playerByClubGateway
        )
        {
            _playerBySeasonGateway = playerBySeasonGateway;
            _playerRepository = playerRepository;
            _playerSeasonRepository = playerSeasonRepository;
            _clubRepository = clubRepository;
            _playerByClubGateway = playerByClubGateway;
        }

        [Command("seed-players", Description = "Seeds players and player-seasons for all seasons from 2000 onward")]
        public async Task SeedPlayers([Option('s')] string source)
        {
            switch (source)
            {
                case "euroleague":
                    await SeedPlayersEuroleague(default);
                    break;
                case "proballers":
                    await SeedPlayersProballers(default);
                    break;
                default:
                    throw new ArgumentException("Unknown source");
            }
        }

        private async Task SeedPlayersProballers(CancellationToken cancellationToken)
        {
            foreach (var clubCode in ProballersClubUriHelper.GetCodes())
            {
                cancellationToken.ThrowIfCancellationRequested();

                Console.WriteLine($"Seeding players for club {clubCode}");
                var (rawPlayerSeasons, gatewayErrors) = await _playerByClubGateway.FetchPlayersSeasonsByClubAsync(clubCode);
                if (gatewayErrors.Any())
                {
                    // Append to log file - player not properly fetched
                    Console.Error.WriteLine($"There are errors for club {clubCode}");
                }

                if (rawPlayerSeasons == null)
                {
                    throw new ArgumentException("No player seasons found");
                }

                await UpsertPlayersAndPlayerSeasons(rawPlayerSeasons, cancellationToken);

                Console.WriteLine($"Finished seeding players for club {clubCode}");
            }
        }

        private async Task SeedPlayersEuroleague(CancellationToken cancellationToken)
        {
            for (int season = 2000; season <= 2023; season++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Console.WriteLine($"Seeding players for season {season}");
                var (rawPlayerSeasons, gatewayErrors) = await _playerBySeasonGateway.FetchPlayerSeasonsBySeasonAsync(season);
                if (gatewayErrors.Any())
                {
                    // Append to log file - player not properly fetched
                    Console.Error.WriteLine($"There are errors for season {season}");
                }

                if (rawPlayerSeasons == null)
                {
                    throw new ArgumentException("No player seasons found");
                }

                await UpsertPlayersAndPlayerSeasons(rawPlayerSeasons, cancellationToken);

                Console.WriteLine($"Finished seeding players for season {season}");
            }
        }

        private async Task UpsertPlayersAndPlayerSeasons(IEnumerable<PlayerSeason> rawPlayerSeasons, CancellationToken cancellationToken)
        {
            try
            {
                var players = rawPlayerSeasons.Select(ps => ps.Player)
                    .Where(p => p != null)
                    .GroupBy(p => new { p!.FirstName, p.LastName, p.BirthDate })
                    .Select(g => g.First())
                    .ToList();

                await _playerRepository.UpsertManyAsync(players!, (p) => new { p.FirstName, p.LastName, p.BirthDate }, (pDb, pIns) => new Player()
                {
                    FirstName = pDb.FirstName,
                    LastName = pDb.LastName,
                    BirthDate = pDb.BirthDate,
                    ImageUrl = pDb.ImageUrl ?? pIns.ImageUrl,
                    Country = pDb.Country ?? pIns.Country,
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }

            var playerSeasons = await PreparePlayerSeasons(rawPlayerSeasons, cancellationToken);
            try
            {
                await _playerSeasonRepository.UpsertManyAsync(playerSeasons, (p) => new { p.PlayerId, p.ClubId, p.Season }, (psDb, psIns) => new PlayerSeason() { }, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<IList<PlayerSeason>> PreparePlayerSeasons(IEnumerable<PlayerSeason> rawPlayerSeasons, CancellationToken cancellationToken)
        {
            var playerSeasons = new List<PlayerSeason>();
            var foundPlayerIds = new HashSet<int>();

            foreach (var ps in rawPlayerSeasons)
            {
                cancellationToken.ThrowIfCancellationRequested();

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
}
