using EL_t3.API.Contracts.Player;
using EL_t3.Application.Player.Payloads;
using EL_t3.Application.Player.Queries;
using EL_t3.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        var query = new PlayerAutocomplete.Query(search);

        var result = await _mediator.Send(query);

        var config = new TypeAdapterConfig();
        config.NewConfig<Player, PlayerDto>()
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

        var playerDtoList = result.Adapt<IEnumerable<PlayerDto>>(config);

        return Ok(playerDtoList);
    }

    [HttpPost("check-constraints/{id}")]
    public async Task<IActionResult> CheckConstraints(int id, [FromBody] IEnumerable<PlayerConstraintPayload> constraints)
    {
        var query = new CheckPlayerConstraints.Query(id, constraints);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // TODO - refactor and move to CLI
    [HttpPost("find-by-constraints")]
    public async Task<IActionResult> FindByConstraints([FromBody] IEnumerable<PlayerConstraintPayload> constraints)
    {
        var query = new FindPlayerByConstraints.Query(constraints);

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}