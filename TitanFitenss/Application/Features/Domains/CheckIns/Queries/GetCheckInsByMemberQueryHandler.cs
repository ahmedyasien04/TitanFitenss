using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.CheckIns.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.CheckIns.Queries;
public record GetCheckInsByMemberQuery(int MemberId):IRequest<List<CheckInDTO>>;
public class GetCheckInsByMemberQueryHandler:IRequestHandler<GetCheckInsByMemberQuery, List<CheckInDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetCheckInsByMemberQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<CheckInDTO>> Handle(GetCheckInsByMemberQuery request, CancellationToken cancellationToken)
    {
        return await _context.CheckIns
            .AsNoTracking()
            .Where(c=>c.MemberId==request.MemberId)
            .OrderByDescending(c=>c.CheckInDateTime)
            .Take(20)
            .Select(c=>new CheckInDTO(
                c.CheckInId, c.MemberId, c.BranchId,
                _context.Branches.Where(b=>b.BranchId==c.BranchId).Select(b=>b.BranchName).First(),
                c.CheckInDateTime, c.Result.ToString(), c.RefusalReason))
            .ToListAsync(cancellationToken);
    }
}
