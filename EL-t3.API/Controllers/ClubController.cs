using EL_t3.Core.Actions.Club.Queries;
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
    //[SwaggerResponse(StatusCodes.Status404NotFound, "The document was not found")]
    //[SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden access to the document")]
    public async Task<IActionResult> GetClubById(int id)
    {
        var query = new GetClubByIdQuery(id);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

}