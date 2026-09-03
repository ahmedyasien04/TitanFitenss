using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.ClassSessions.Commands;
using TitanFitenss.Application.Features.Domains.ClassSessions.Queries;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassSessionsController : ControllerBase
{
    private readonly ISender _mediator;
    public ClassSessionsController(ISender mediator)
    {
        _mediator=mediator;
    }

    // POST/api/classsessions
    [HttpPost]
    public async Task<IActionResult> Schedule(ScheduleClassSessionCommand command, CancellationToken ct)
    {
        var sessionId = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id=sessionId },new {sessionId});
    }

    // GET/api/classsessions?branchId=1&date=2026/9/5 
    [HttpGet]
    public async Task<IActionResult> GetByBranchAndDate(
        [FromQuery] int branchId, [FromQuery] DateOnly date, CancellationToken ct)
    {
        var sessions=await _mediator.Send(new GetSessionsByBranchAndDateQuery(branchId,date), ct);
        return Ok(sessions);
    }
    // GET /api/classsessions/id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var session=await _mediator.Send(new GetSessionByIdQuery(id), ct);
        return Ok(session);
    }
    // POST /api/classsessions/id/bookings 
    [HttpPost("{id:int}/bookings")]
    public async Task<IActionResult> BookMember(int id, BookMemberRequest request, CancellationToken ct)
    {
        var bookingId=await _mediator.Send(
            new BookMemberCommand(id, request.MemberId, request.NotesForTrainer),ct);

        return CreatedAtAction(nameof(GetById),new{id},new{bookingId});
    }
    // DELETE /api/classsessions/id/bookings/id
    [HttpDelete("{id:int}/bookings/{bookingId:int}")]
    public async Task<IActionResult> CancelBooking(int id, int bookingId, CancellationToken ct)
    {
        await _mediator.Send(new CancelBookingCommand(id, bookingId), ct);
        return NoContent();
    }
    // POST /api/classsessions/id/cancel
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> CancelSession(int id, CancellationToken ct)
    {
        await _mediator.Send(new CancelClassSessionCommand(id), ct);
        return NoContent();
    }
}
public record BookMemberRequest(int MemberId, string? NotesForTrainer);
