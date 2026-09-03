using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.Branches.Commands;
using TitanFitenss.Application.Features.Domains.Branches.Queries;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchesController:ControllerBase
{
    private readonly ISender _mediator;
    public BranchesController(ISender mediator)
    {
        _mediator = mediator;
    }
    //POST/api/branches
    [HttpPost]
    public async Task<IActionResult> Create(CreateBranchCommand command, CancellationToken ct)
    {
        var branchId=await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new {id=branchId},new { branchId });
    }
    // GET/api/branches
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var branches=await _mediator.Send(new GetBranchesListQuery(),ct);
        return Ok(branches);
    }
    // GET/api/branches/id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var branch=await _mediator.Send(new GetBranchByIdQuery(id),ct);
        return Ok(branch);
    }
    // GET/api/branches/lookup
    [HttpGet("lookup")]
    public async Task<IActionResult> GetLookup(CancellationToken ct)
    {
        var branches=await _mediator.Send(new GetBranchesLookupQuery(),ct);
        return Ok(branches);
    }
}
