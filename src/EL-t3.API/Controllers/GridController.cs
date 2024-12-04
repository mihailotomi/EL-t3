using EL_t3.Application.Grid.Queries;
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
        var query = new GetRandomGrid.Query();

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}