using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Models;
using TitanFitenss.Application.Features.Domains.Members.DTOs;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Members.Queries;
public record GetMembersListQuery(
    string? SearchTerm,
    int? BranchId,
    int PageNumber=1,
    int PageSize=20
):IRequest<PaginatedList<MemberListItemDTO>>;

public class GetMembersListQueryHandler
    :IRequestHandler<GetMembersListQuery, PaginatedList<MemberListItemDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetMembersListQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<PaginatedList<MemberListItemDTO>> Handle(
        GetMembersListQuery request, CancellationToken cancellationToken)
    {
        var query=_context.Members.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term=request.SearchTerm.Trim();
            query=query.Where(m=>
                m.FullName.Contains(term)||m.MembershipNumber.Value.Contains(term));
        }

        if (request.BranchId.HasValue)
        {
            query=query.Where(m=>m.HomeBranchId==request.BranchId.Value);
        }

        var projected=query
            .OrderBy(m=>m.FullName)
            .Select(m=>new MemberListItemDTO(
                m.MemberId,
                m.MembershipNumber.Value,
                m.FullName,
                _context.Branches.Where(b=>b.BranchId==m.HomeBranchId).Select(b=>b.BranchName).First(),
                _context.Memberships
                    .Where(ms=>ms.MemberId==m.MemberId)
                    .OrderByDescending(ms=>ms.StartDate)
                    .Select(ms=>ms.Status.ToString())
                    .FirstOrDefault()
            ));

        return await PaginatedList<MemberListItemDTO>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}
