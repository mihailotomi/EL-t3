using EL_t3.API.Contracts.Player;
using EL_t3.Core.Actions.Grid.Queries.GetRandomGrid;
using EL_t3.Core.Actions.Player.Queries.CheckConstraints;
using EL_t3.Core.Actions.Player.Queries.PlayerAutocomplete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EL_t3.API.Controllers;

[Route("grids")]
public class GridController : Controller
{
    private readonly IMediator _mediator;

    public GridController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandom()
    {
        var query = new GetRandomGridQuery();

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}