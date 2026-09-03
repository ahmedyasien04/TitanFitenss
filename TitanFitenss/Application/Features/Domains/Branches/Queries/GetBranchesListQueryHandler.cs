using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Branches.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Branches.Queries;
public record GetBranchesListQuery:IRequest<List<BranchDTO>>;
public class GetBranchesListQueryHandler:IRequestHandler<GetBranchesListQuery, List<BranchDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetBranchesListQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<BranchDTO>> Handle(GetBranchesListQuery request,CancellationToken cancellationToken)
    {
        return await _context.Branches
            .AsNoTracking()
            .OrderBy(b=>b.BranchName)
            .Select(b=>new BranchDTO(
                b.BranchId,
                b.BranchName,
                b.OpeningTime,
                b.ClosingTime,
                b.Address.City,
                b.Address.Street,
                b.Address.ApartmentNumber,
                b.Studios.Select(s=>new StudioDTO(s.StudioId,s.StudioName,s.Capacity)).ToList()
            ))
            .ToListAsync(cancellationToken);
    }
}
