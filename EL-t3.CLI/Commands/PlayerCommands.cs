using Cocona;
using Cocona.Application;
using EL_t3.Application.Player.Commands;
using EL_t3.Application.Player.Queries;
using EL_t3.Infrastructure.Gateway.Helpers;
using MediatR;

namespace EL_t3.CLI.Commands
{
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
            var command = new FindDisjointPlayers.Query();
            // TODO - store errors in file
            var result = await _mediator.Send(command, ctx.CancellationToken);
            foreach (var pair in result)
            {
                Console.WriteLine($"Disjoint player: id-{pair.Player1.Id} {pair.Player1.FirstName} {pair.Player1.LastName} " +
                    $"and id-{pair.Player2.Id} {pair.Player2.FirstName} {pair.Player2.LastName}");
            }
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
    }
}
