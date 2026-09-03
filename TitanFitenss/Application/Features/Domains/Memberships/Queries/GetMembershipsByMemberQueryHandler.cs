using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Memberships.DTOs;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Memberships.Queries;
public record GetMembershipsByMemberQuery(int MemberId):IRequest<List<MembershipDTO>>;
public class GetMembershipsByMemberQueryHandler
    :IRequestHandler<GetMembershipsByMemberQuery, List<MembershipDTO>>
{
    private readonly TitanFitnessDbContext _context;

    public GetMembershipsByMemberQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<MembershipDTO>> Handle(
        GetMembershipsByMemberQuery request, CancellationToken cancellationToken)
    {
        return await _context.Memberships
            .AsNoTracking()
            .Where(m=>m.MemberId==request.MemberId)
            .OrderByDescending(m=>m.StartDate)
            .Select(m=>new MembershipDTO(
                m.MembershipId,
                m.MemberId,
                m.PlanId,
                _context.Plans.Where(p=>p.PlanId==m.PlanId).Select(p=>p.PlanName).First(),
                m.PurchaseDate,
                m.StartDate,
                m.EndDate,
                m.Status.ToString(),
                m.AgreedTerms.PricePaid,
                m.AgreedTerms.DurationInMonths,
                m.AgreedTerms.MaxFreezeDays,
                m.AgreedTerms.MaxNumberOfFreezeDays,
                m.AgreedTerms.GuestPassQuota,
                m.AgreedTerms.AccessScope.ToString(),
                m.Freezes.Select(f=>new FreezeDTO(
                    f.FreezeId, f.StartDate, f.EndDate, f.DurationInMonths,
                    f.Reason.ToString(), f.AdditionalNotes, f.RequestedOn)).ToList(),
                m.GuestPasses.Select(g=>new GuestPassDTO(
                    g.GuestPassId, g.IssuedOn, g.UsedOn, g.GuestName)).ToList()
            ))
            .ToListAsync(cancellationToken);
    }
}
