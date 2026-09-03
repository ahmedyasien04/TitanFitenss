using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.Dashboard.Queries;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController:ControllerBase
{
    private readonly ISender _mediator;

    public DashboardController(ISender mediator)
    {
        _mediator=mediator;
    }
    // GET/api/dashboard?branchId=1 
    [HttpGet]
    public async Task<IActionResult> GetStats([FromQuery] int? branchId, CancellationToken ct)
    {
        var stats=await _mediator.Send(new GetDashboardStatsQuery(branchId), ct);
        return Ok(stats);
    }
}
