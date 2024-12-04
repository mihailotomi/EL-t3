using Cocona;
using Cocona.Application;
using Cocona.Builder;
using EL_t3.Application.Player.Commands;
using EL_t3.Application.Player.Queries;
using EL_t3.CLI.Helpers;
using EL_t3.Infrastructure.Gateway.Helpers;
using MediatR;

namespace EL_t3.CLI.Commands;

public static class PlayerCommands
{
    public static void Commands(ICoconaCommandsBuilder builder)
    {
        builder.AddSubCommand("seed", SeedPlayerSeasons);
        builder.AddCommand("find-disjoint", FindDisjointPlayers).WithDescription("Finds possible disjoint players - the same player under 2 records due to data coruption on some source.");
        builder.AddCommand("merge", MergePlayers).WithDescription("Merges 2 players with all seasons (used when disjoint players are found).");
    }

    public static void SeedPlayerSeasons(ICoconaCommandsBuilder builder)
    {
        builder.AddCommand("euroleague", SeedPlayersEuroleague).WithDescription("Seeds all Euroleague players and player-seasons for all seasons from 2000 onward.");
        builder.AddCommand("nba", SeedPlayersNba).WithDescription("Seeds all NBA seasons for players who played in the Euroleague.");
    }

    public static async Task SeedPlayersEuroleague([Option('s')] string source, [FromService] ICoconaAppContextAccessor contextAccessor, [FromService] IMediator mediator)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();

        switch (source)
        {
            case "euroleague-api":
                await SeedEuroleaguePlayersEuroleagueApi(mediator, ctx.CancellationToken);
                break;
            case "proballers":
                await SeedEuroleaguePlayersProballers(mediator, ctx.CancellationToken);
                break;
            default:
                throw new ArgumentException("Unknown source");
        }
    }

    public static async Task SeedPlayersNba([FromService] ICoconaAppContextAccessor contextAccessor, [FromService] IMediator mediator)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();
        var uriHelper = new ProballersNbaUriHelper();

        var command = new SeedNbaPlayerSeasonsByClub.Command(uriHelper.GetCodes());
        // TODO - store errors in file
        var result = await mediator.Send(command, ctx.CancellationToken);
    }

    public static async Task FindDisjointPlayers([FromService] IMediator mediator, [FromService] ICoconaAppContextAccessor contextAccessor)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();
        var query = new FindDisjointPlayers.Query();
        // TODO - store errors in file
        var result = await mediator.Send(query, ctx.CancellationToken);
        foreach (var pair in result)
        {
            ConsoleFormatHelper.ConsoleDisjointPlayer(pair);
        }
    }

    [Command("merge-players", Description = "Merges 2 players with all seasons (used when disjoint players are found)")]
    public static async Task MergePlayers([Option("p1")] string player1,
        [Option("p2")] string player2,
        [Option("d")] string personalDataFrom,
        [FromService] IMediator mediator,
        [FromService] ICoconaAppContextAccessor contextAccessor)
    {
        if (!int.TryParse(player1, out int p1Id) || p1Id < 1)
        {
            Console.WriteLine("Wrong format for player 1 id!");
            return;
        }
        if (!int.TryParse(player2, out int p2Id) || p2Id < 1)
        {
            Console.WriteLine("Wrong format for player 2 id!");
            return;
        }
        if (!int.TryParse(personalDataFrom, out int dataIndex) || dataIndex > 2 || dataIndex < 1)
        {
            Console.WriteLine("Personal data from must be 1 or 2!");
            return;
        }

        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();
        var command = new MergePlayers.Command(p1Id, p2Id, dataIndex);
        try
        {
            var result = await mediator.Send(command, ctx.CancellationToken);
            ConsoleFormatHelper.ConsoleMergedPlayer(result!);
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }

    }

    private static async Task SeedEuroleaguePlayersProballers(IMediator mediator, CancellationToken cancellationToken)
    {
        var uriHelper = new ProballersEuroleagueUriHelper();

        var command = new SeedPlayersByClub.Command(uriHelper.GetCodes());
        // TODO - store errors in file
        var result = await mediator.Send(command, cancellationToken);
    }

    private static async Task SeedEuroleaguePlayersEuroleagueApi(IMediator mediator, CancellationToken cancellationToken)
    {
        var command = new SeedPlayersBySeason.Command();
        // TODO - store errors in file
        var result = await mediator.Send(command, cancellationToken);
    }
}

