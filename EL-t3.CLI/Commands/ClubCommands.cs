using Cocona;
using EL_t3.Application.Club.Commands;
using MediatR;

namespace EL_t3.CLI.Commands;

public class ClubCommands
{
    private readonly IMediator _mediator;

    public ClubCommands(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Command("seed-clubs", Description = "Seeds clubs for all seasons from 2000 onward")]
    public async Task SeedClubs()
    {
        var command = new SeedClubs.Command();
        var result = await _mediator.Send(command);
    }
}