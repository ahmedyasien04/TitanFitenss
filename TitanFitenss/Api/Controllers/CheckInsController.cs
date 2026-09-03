using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.CheckIns.Commands;
using TitanFitenss.Application.Features.Domains.CheckIns.Queries;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckInsController:ControllerBase
{
    private readonly ISender _mediator;
    public CheckInsController(ISender mediator)
    {
        _mediator=mediator;
    }
    // POST /api/checkins
    [HttpPost]
    public async Task<IActionResult> CheckIn(CheckInMemberCommand command, CancellationToken ct)
    {
        var result=await _mediator.Send(command, ct);
        return Ok(result);
    }
    // GET /api/checkins/member/id
    [HttpGet("member/{memberId:int}")]
    public async Task<IActionResult> GetByMember(int memberId, CancellationToken ct)
    {
        var checkIns=await _mediator.Send(new GetCheckInsByMemberQuery(memberId),ct);
        return Ok(checkIns);
    }
}
