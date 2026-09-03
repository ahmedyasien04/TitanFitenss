using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.Memberships.Commands;
using TitanFitenss.Application.Features.Domains.Memberships.Queries;
using TitanFitenss.Domain.MembershipAggregate;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembershipsController:ControllerBase
{
    private readonly ISender _mediator;

    public MembershipsController(ISender mediator)
    {
        _mediator=mediator;
    }

    // POST /api/memberships 
    [HttpPost]
    public async Task<IActionResult> Purchase(PurchaseMembershipCommand command, CancellationToken ct)
    {
        var membershipId = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new{id=membershipId},new{membershipId});
    }
    // GET /api/memberships/id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var membership=await _mediator.Send(new GetMembershipByIdQuery(id), ct);
        return Ok(membership);
    }
    // GET /api/memberships?memberId
    [HttpGet]
    public async Task<IActionResult> GetByMember([FromQuery] int memberId, CancellationToken ct)
    {
        var memberships=await _mediator.Send(new GetMembershipsByMemberQuery(memberId), ct);
        return Ok(memberships);
    }
    // POST /api/memberships/id/freeze  
    [HttpPost("{id:int}/freeze")]
    public async Task<IActionResult> RequestFreeze(int id, RequestFreezeRequest request, CancellationToken ct)
    {
        var command=new RequestFreezeCommand(
            id, request.StartDate, request.DurationInMonths, request.Reason, request.AdditionalNotes);
        await _mediator.Send(command, ct);
        return NoContent();
    }
    // POST /api/memberships/id/cancel
    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        await _mediator.Send(new CancelMembershipCommand(id), ct);
        return NoContent();
    }
    // POST /api/memberships/id/change-plan  
    [HttpPost("{id:int}/change-plan")]
    public async Task<IActionResult> ChangePlan(int id, ChangePlanRequest request, CancellationToken ct)
    {
        var newMembershipId=await _mediator.Send(
            new ChangeMembershipPlanCommand(id,request.NewPlanId,request.EffectiveImmediately),ct);

        return CreatedAtAction(nameof(GetById),new{id=newMembershipId},new{membershipId=newMembershipId});
    }
    // POST /api/memberships/id/guest-passes/id/use
    [HttpPost("{id:int}/guest-passes/{guestPassId:int}/use")]
    public async Task<IActionResult> UseGuestPass(
        int id, int guestPassId, UseGuestPassRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UseGuestPassCommand(id,guestPassId,request.GuestName),ct);
        return NoContent();
    }
}
public record RequestFreezeRequest(DateOnly StartDate,int DurationInMonths,Reason Reason,string? AdditionalNotes);
public record ChangePlanRequest(int NewPlanId,bool EffectiveImmediately);
public record UseGuestPassRequest(string GuestName);
