using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.Trainers.Commands;
using TitanFitenss.Application.Features.Domains.Trainers.Queries;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainersController:ControllerBase
{
    private readonly ISender _mediator;

    public TrainersController(ISender mediator)
    {
        _mediator=mediator;
    }
    // POST /api/trainers 
    [HttpPost]
    public async Task<IActionResult> Create(CreateTrainerCommand command, CancellationToken ct)
    {
        var trainerId=await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById),new{id=trainerId},new{trainerId});
    }
    // GET /api/trainers
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int pageNumber=1,[FromQuery] int pageSize=20,CancellationToken ct=default)
    {
        var result=await _mediator.Send(new GetTrainersListQuery(pageNumber,pageSize),ct);
        return Ok(result);
    }
    // GET /api/trainers/id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var trainer=await _mediator.Send(new GetTrainerByIdQuery(id), ct);
        return Ok(trainer);
    }
    // GET /api/trainers/active/lookup 
    [HttpGet("active/lookup")]
    public async Task<IActionResult> GetActiveLookup(CancellationToken ct)
    {
        var trainers=await _mediator.Send(new GetActiveTrainersLookupQuery(), ct);
        return Ok(trainers);
    }
    // PUT /api/trainers/id
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTrainerRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateTrainerCommand(id,request.TrainerName,request.Email,request.Phone),ct);
        return NoContent();
    }
    // PATCH /api/trainers/id/active  
    [HttpPatch("{id:int}/active")]
    public async Task<IActionResult> SetActive(int id, SetActiveRequest request, CancellationToken ct)
    {
        await _mediator.Send(new SetTrainerActiveCommand(id, request.IsActive), ct);
        return NoContent();
    }
}
public record UpdateTrainerRequest(string TrainerName, string Email, string Phone);
public record SetActiveRequest(bool IsActive);
