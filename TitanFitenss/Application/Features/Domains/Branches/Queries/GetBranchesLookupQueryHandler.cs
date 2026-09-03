using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Branches.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Branches.Queries;
public record GetBranchesLookupQuery:IRequest<List<BranchLookUpDTO>>;

public class GetBranchesLookupQueryHandler:IRequestHandler<GetBranchesLookupQuery,List<BranchLookUpDTO>>
{
    private readonly TitanFitnessDbContext _context;
    public GetBranchesLookupQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<BranchLookUpDTO>> Handle(GetBranchesLookupQuery request, CancellationToken cancellationToken)
    {
        return await _context.Branches
            .AsNoTracking()
            .OrderBy(b=>b.BranchName)
            .Select(b=>new BranchLookUpDTO(b.BranchId, b.BranchName))
            .ToListAsync(cancellationToken);
    }
}
