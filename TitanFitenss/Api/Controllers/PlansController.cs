using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.Plans.Commands;
using TitanFitenss.Application.Features.Domains.Plans.Queries;
using TitanFitenss.Domain.ValueObjects;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlansController:ControllerBase
{
    private readonly ISender _mediator;

    public PlansController(ISender mediator)
    {
        _mediator=mediator;
    }
    // POST /api/plans 
    [HttpPost]
    public async Task<IActionResult> Create(CreatePlanCommand command, CancellationToken ct)
    {
        var planId=await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById),new{id=planId},new{planId});
    }
    // GET /api/plans  
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int pageNumber=1,[FromQuery] int pageSize=20,CancellationToken ct=default)
    {
        var result=await _mediator.Send(new GetPlansListQuery(pageNumber,pageSize),ct);
        return Ok(result);
    }
    // GET /api/plans/id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var plan=await _mediator.Send(new GetPlanByIdQuery(id), ct);
        return Ok(plan);
    }
    // GET /api/plans/published/lookup  
    [HttpGet("published/lookup")]
    public async Task<IActionResult> GetPublishedLookup(CancellationToken ct)
    {
        var plans=await _mediator.Send(new GetPublishedPlansLookupQuery(), ct);
        return Ok(plans);
    }
    // PUT /api/plans/id
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id,UpdatePlanRequest request,CancellationToken ct)
    {
        var command=new UpdatePlanCommand(
            id,request.PlanName,request.Price,request.DurationInMonths,
            request.MaxFreezeDays,request.MaxNumberOfFreezes,request.GuestPassQuota,request.AccessScope);
        await _mediator.Send(command, ct);
        return NoContent();
    }

    // PATCH /api/plans/id/publish   
    [HttpPatch("{id:int}/publish")]
    public async Task<IActionResult> SetPublished(int id, SetPublishedRequest request, CancellationToken ct)
    {
        await _mediator.Send(new SetPlanPublishedCommand(id, request.IsPublished), ct);
        return NoContent();
    }
}
public record UpdatePlanRequest(
    string PlanName,decimal Price,int DurationInMonths,int MaxFreezeDays,
    int MaxNumberOfFreezes,int GuestPassQuota,AccessScope AccessScope);
public record SetPublishedRequest(bool IsPublished);
