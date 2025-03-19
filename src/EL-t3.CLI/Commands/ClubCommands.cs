using Cocona;
using Cocona.Application;
using Cocona.Builder;
using EL_t3.Application.Club.Commands;
using MediatR;

namespace EL_t3.CLI.Commands;



public static class ClubCommands
{
    public static void Commands(ICoconaCommandsBuilder builder)
    {
        builder.AddSubCommand("seed", SeedClubs);
    }

    public static void SeedClubs(ICoconaCommandsBuilder builder)
    {
        builder.AddCommand("euroleague", SeedClubsEuroleague).WithDescription("Seeds all Euroleague clubs for all seasons from 2000 onward.");
        builder.AddCommand("nba", SeedClubsNba).WithDescription("Seeds all 30 NBA clubs.");
    }

    public static async Task SeedClubsEuroleague([FromService] ICoconaAppContextAccessor contextAccessor, [FromService] IMediator mediator)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();

        var command = new SeedClubsEuroleague.Command();
        var result = await mediator.Send(command, ctx.CancellationToken);
    }

    public static async Task SeedClubsNba([FromService] ICoconaAppContextAccessor contextAccessor, [FromService] IMediator mediator)
    {
        var ctx = contextAccessor.Current ?? throw new InvalidOperationException();

        var command = new SeedClubsNba.Command();
        var result = await mediator.Send(command, ctx.CancellationToken);
    }
}