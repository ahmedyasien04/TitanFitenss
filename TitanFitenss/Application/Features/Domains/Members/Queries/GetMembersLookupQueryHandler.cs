using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Members.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Members.Queries;

public record GetMembersLookupQuery(string? SearchTerm):IRequest<List<MemberLookupDTO>>;

public class GetMembersLookupQueryHandler:IRequestHandler<GetMembersLookupQuery, List<MemberLookupDTO>>
{
    private readonly TitanFitnessDbContext _context;
    public GetMembersLookupQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }

    public async Task<List<MemberLookupDTO>> Handle(GetMembersLookupQuery request, CancellationToken cancellationToken)
    {
        var query=_context.Members.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term=request.SearchTerm.Trim();
            query=query.Where(m=>m.FullName.Contains(term)||m.MembershipNumber.Value.Contains(term));
        }

        return await query
            .OrderBy(m=>m.FullName)
            .Take(20)
            .Select(m=>new MemberLookupDTO(m.MemberId, m.MembershipNumber.Value, m.FullName))
            .ToListAsync(cancellationToken);
    }
}
