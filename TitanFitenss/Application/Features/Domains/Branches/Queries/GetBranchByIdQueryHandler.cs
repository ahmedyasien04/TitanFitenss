using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.Branches.DTOs;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Infrastructure.Persistence;

namespace TitanFitenss.Application.Features.Domains.Branches.Queries;

public record GetBranchByIdQuery(int BranchId):IRequest<BranchDTO>;

public class GetBranchByIdQueryHandler:IRequestHandler<GetBranchByIdQuery, BranchDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetBranchByIdQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<BranchDTO> Handle(GetBranchByIdQuery request,CancellationToken cancellationToken)
    {
        var branch=await _context.Branches
            .AsNoTracking()
            .Where(b=>b.BranchId==request.BranchId)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (branch is null)
            throw new NotFoundException(nameof(Branch), request.BranchId);

        return branch;
    }
}
