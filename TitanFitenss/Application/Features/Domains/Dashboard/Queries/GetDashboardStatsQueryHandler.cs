using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.Dashboard.DTOs;
using TitanFitenss.Domain.CheckInAggregate;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.Dashboard.Queries;
public record GetDashboardStatsQuery(int? BranchId):IRequest<DashboardStatsDTO>;

public class GetDashboardStatsQueryHandler:IRequestHandler<GetDashboardStatsQuery, DashboardStatsDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetDashboardStatsQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<DashboardStatsDTO> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var today=DateTime.UtcNow.Date;
        var tomorrow=today.AddDays(1);

        var checkIns=_context.CheckIns
            .AsNoTracking()
            .Where(c=>c.CheckInDateTime>=today&&c.CheckInDateTime<tomorrow
                &&c.Result==CheckInResult.Granted);

        var activeMemberships=_context.Memberships
            .AsNoTracking()
            .Where(m=>m.Status==MembershipStatus.Active);

        var sessionsToday=_context.ClassSessions
            .AsNoTracking()
            .Where(s=>s.SessionDate==today);

        if (request.BranchId.HasValue)
        {
            checkIns=checkIns.Where(c=>c.BranchId==request.BranchId.Value);
            sessionsToday=sessionsToday.Where(s=>s.BranchId==request.BranchId.Value);
            activeMemberships=activeMemberships.Where(m=>
                _context.Members.Any(mem=>mem.MemberId==m.MemberId&&mem.HomeBranchId==request.BranchId.Value));
        }

        var checkInsToday=await checkIns.CountAsync(cancellationToken);
        var activeMembershipsCount=await activeMemberships.CountAsync(cancellationToken);

        var todaysSessions=await sessionsToday
            .OrderBy(s=>s.StartTime)
            .Select(s=>new TodaysSessionDTO(
                s.SessionId,
                s.ClassName,
                s.StartTime,
                _context.Branches.SelectMany(b=>b.Studios)
                    .Where(st=>st.StudioId==s.StudioId).Select(st=>st.StudioName).First(),
                _context.Trainers.Where(t=>t.TrainerId==s.TrainerId).Select(t=>t.TrainerName).First(),
                s.Bookings.Count(b=>b.Status==Domain.ClassSessionAggregate.BookingStatus.Confirmed),
                s.CapacityLimit,
                s.Status.ToString()
            ))
            .ToListAsync(cancellationToken);

        var totalBookingsToday=todaysSessions.Sum(s=>s.ConfirmedCount);
        var totalCapacityToday=todaysSessions.Sum(s=>s.CapacityLimit);
        var averageFillRate=totalCapacityToday==0
            ? 0
            :Math.Round(totalBookingsToday*100.0/totalCapacityToday,1);

        return new DashboardStatsDTO(
            checkInsToday,activeMembershipsCount,totalBookingsToday,averageFillRate,todaysSessions);
    }
}
