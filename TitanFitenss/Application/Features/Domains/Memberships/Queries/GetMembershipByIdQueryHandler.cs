using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.Memberships.DTOs;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Memberships.Queries;
public record GetMembershipByIdQuery(int MembershipId):IRequest<MembershipDTO>;

public class GetMembershipByIdQueryHandler:IRequestHandler<GetMembershipByIdQuery, MembershipDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetMembershipByIdQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }

    public async Task<MembershipDTO> Handle(GetMembershipByIdQuery request, CancellationToken cancellationToken)
    {
        var dto=await _context.Memberships
            .AsNoTracking()
            .Where(m=>m.MembershipId==request.MembershipId)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            throw new NotFoundException(nameof(Membership), request.MembershipId);

        return dto;
    }
}
