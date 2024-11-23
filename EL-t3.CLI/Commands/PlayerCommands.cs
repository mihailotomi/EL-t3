using Cocona;
using Cocona.Application;
using EL_t3.Application.Player.Commands;
using EL_t3.Application.Player.Queries;
using EL_t3.Domain.Entities;
using EL_t3.Infrastructure.Gateway.Helpers;
using MediatR;

namespace EL_t3.CLI.Commands;

public class PlayerCommands
{
    private readonly IMediator _mediator;

    public PlayerCommands(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Command("seed-players", Description = "Seeds players and player-seasons for all seasons from 2000 onward")]
    public async Task SeedPlayers([Option('s')] string source, [FromService] ICoconaAppContextAccessor contextAccessor)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();

        switch (source)
        {
            case "euroleague":
                await SeedPlayersEuroleague(ctx.CancellationToken);
                break;
            case "proballers":
                await SeedPlayersProballers(ctx.CancellationToken);
                break;
            default:
                throw new ArgumentException("Unknown source");
        }
    }

    [Command("disjoint-players", Description = "Finds possible disjoint players - the same player under 2 records due to data coruption on some source")]
    public async Task FindDisjointPlayers([FromService] ICoconaAppContextAccessor contextAccessor)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();
        var query = new FindDisjointPlayers.Query();
        // TODO - store errors in file
        var result = await _mediator.Send(query, ctx.CancellationToken);
        foreach (var pair in result)
        {
            ConsoleDisjointPlayer(pair);
        }
    }

    [Command("merge-players", Description = "Merges 2 players with all seasons (used when disjoint players are found)")]
    public async Task MergePlayers([Option("p1")] string player1,
        [Option("p2")] string player2,
        [Option("d")] string personalDataFrom,
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
            var result = await _mediator.Send(command, ctx.CancellationToken);
            ConsoleMergedPlayer(result!);
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }

    }

    private async Task SeedPlayersProballers(CancellationToken cancellationToken)
    {
        var command = new SeedPlayersByClub.Command(ProballersClubUriHelper.GetCodes());
        // TODO - store errors in file
        var result = await _mediator.Send(command, cancellationToken);
    }

    private async Task SeedPlayersEuroleague(CancellationToken cancellationToken)
    {
        var command = new SeedPlayersBySeason.Command();
        // TODO - store errors in file
        var result = await _mediator.Send(command, cancellationToken);
    }

    private static void ConsoleMergedPlayer(Player player)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Merged player:");

        ConsoleDataDetail("Name:", $"{player.FirstName} {player.LastName}");
        ConsoleDataDetail("Country:", $"{player.Country}");
        ConsoleDataDetail("Birth:", $"{player.BirthDate}");
        ConsoleDataDetail("Seasons:", "");

        foreach (var seasonPlayed in player.SeasonsPlayed!)
        {
            ConsoleDataDetail($"  {seasonPlayed.Season}", seasonPlayed.Club!.Name, ConsoleColor.Yellow);
        }
    }

    private static void ConsoleDataDetail(string detail, string data, ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.ForegroundColor = color;
        Console.Write($"\t{detail,-10} ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(data);
    }

    private static void ConsoleDisjointPlayer(DisjointPlayers pair)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Possible disjoint player:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\tid--{pair.Player1.Id,-7}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{pair.Player1.FirstName} {pair.Player1.LastName}");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\tid--{pair.Player2.Id,-7}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{pair.Player2.FirstName} {pair.Player2.LastName}");
        Console.Write("\n");
    }
}

