using EL_t3.API.Contracts.Player;
using EL_t3.Core.Actions.Player.Queries.CheckConstraints;
using EL_t3.Core.Actions.Player.Queries.PlayerAutocomplete;
using MediatR;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using EL_t3.Core.Entities;

namespace EL_t3.API.Controllers;

[Route("players")]
public class PlayerController : Controller
{
    private readonly IMediator _mediator;

    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> NameAutocomplete(string search)
    {
        var query = new PlayerAutocompleteQuery(search);

        var result = await _mediator.Send(query);

        var config = new TypeAdapterConfig();
        config.NewConfig<Player, PlayerDto>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

        var playerDtoList = result.Adapt<IEnumerable<PlayerDto>>(config);

        return Ok(playerDtoList);
    }

    [HttpPost("check-constraints/{id}")]
    public async Task<IActionResult> CheckConstraints(int id, [FromBody] IEnumerable<PlayerConstraintDto> constraints)
    {
        var query = new CheckPlayerConstraintsQuery(id, constraints.Select(x => x.GetSubtype()));

        var result = await _mediator.Send(query);
        return Ok(result);
    }

}