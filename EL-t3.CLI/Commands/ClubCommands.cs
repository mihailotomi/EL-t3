using Cocona;
using EL_t3.Core.Interfaces.Gateway;
using EL_t3.Core.Interfaces.Repository;

namespace EL_t3.CLI.Commands;

public class ClubCommands
{
    private readonly IClubGateway _clubGateway;
    private readonly IClubRepository _clubRepository;

    public ClubCommands(IClubGateway clubGateway, IClubRepository clubRepository)
    {
        _clubGateway = clubGateway;
        _clubRepository = clubRepository;
    }

    [Command("seed-clubs", Description = "Seeds clubs for all seasons from 2000 onward")]
    public async Task SeedClubs()
    {
        IEnumerable<int> seasons = Enumerable.Range(2000, 2023 - 2000 + 1);
        foreach (int season in seasons)
        {
            Console.WriteLine($"Seeding clubs for season {season}");
            var (clubs, errors) = await _clubGateway.FetchClubsBySeasonAsync(season);
            if (errors.Any())
            {
                Console.Error.WriteLine($"There are errors for season {season}");
            }

            await _clubRepository.UpsertManyAsync(clubs, (c) => new { c.Code });
            Console.WriteLine($"Finished seeding clubs for season {season}");
        }
    }
}