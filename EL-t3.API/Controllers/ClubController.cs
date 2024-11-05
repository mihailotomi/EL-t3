using EL_t3.Application.Club.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EL_t3.API.Controllers;

[Route("clubs")]
public class ClubController : Controller
{
    private readonly IMediator _mediator;

    public ClubController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClubById(int id)
    {
        var query = new GetClubById.Query(id);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

}