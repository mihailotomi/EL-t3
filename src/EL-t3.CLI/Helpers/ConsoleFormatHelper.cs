using EL_t3.Application.Player.Queries;
using EL_t3.Domain.Entities;

namespace EL_t3.CLI.Helpers;

public static class ConsoleFormatHelper
{
    public static void ConsoleMergedPlayer(Player player)
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

    public static void ConsoleDataDetail(string detail, string data, ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.ForegroundColor = color;
        Console.Write($"\t{detail,-10} ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(data);
    }

    public static void ConsoleDisjointPlayer(DisjointPlayers pair)
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

