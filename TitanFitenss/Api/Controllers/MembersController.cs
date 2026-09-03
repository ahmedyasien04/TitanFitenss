using MediatR;
using Microsoft.AspNetCore.Mvc;
using TitanFitenss.Application.Features.Domains.Members.Commands;
using TitanFitenss.Application.Features.Domains.Members.Queries;
namespace TitanFitenss.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController:ControllerBase
{
    private readonly ISender _mediator;

    public MembersController(ISender mediator)
    {
        _mediator=mediator;
    }
    //POST/api/members  
    [HttpPost]
    public async Task<IActionResult> Register(RegisterMemberCommand command, CancellationToken ct)
    {
        var memberId=await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new{id=memberId},new{memberId});
    }
    // GET /api/members
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] string? searchTerm, [FromQuery] int? branchId,
        [FromQuery] int pageNumber=1, [FromQuery] int pageSize=20,
        CancellationToken ct=default)
    {
        var result=await _mediator.Send(
            new GetMembersListQuery(searchTerm, branchId, pageNumber, pageSize), ct);
        return Ok(result);
    }
    // GET /api/members/5  
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var member=await _mediator.Send(new GetMemberByIdQuery(id), ct);
        return Ok(member);
    }

    // GET /api/members/lookup  
    [HttpGet("lookup")]
    public async Task<IActionResult> GetLookup([FromQuery] string? searchTerm, CancellationToken ct)
    {
        var members=await _mediator.Send(new GetMembersLookupQuery(searchTerm), ct);
        return Ok(members);
    }
    // PUT /api/members/id
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProfile(
        int id, UpdateMemberProfileRequest request, CancellationToken ct)
    {
        var command=new UpdateMemberProfileCommand(
            id,request.FullName,request.Email,request.Phone,
            request.City,request.Street,request.ApartmentNumber,request.Photo);

        await _mediator.Send(command, ct);
        return NoContent();
    }
    // PATCH /api/members/id/home-branch
    [HttpPatch("{id:int}/home-branch")]
    public async Task<IActionResult> ChangeHomeBranch(
        int id, ChangeHomeBranchRequest request, CancellationToken ct)
    {
        await _mediator.Send(new ChangeMemberHomeBranchCommand(id, request.NewBranchId), ct);
        return NoContent();
    }
}
public record UpdateMemberProfileRequest(
    string FullName, string Email, string Phone,
    string City, string Street, int ApartmentNumber, string? Photo);
public record ChangeHomeBranchRequest(int NewBranchId);
