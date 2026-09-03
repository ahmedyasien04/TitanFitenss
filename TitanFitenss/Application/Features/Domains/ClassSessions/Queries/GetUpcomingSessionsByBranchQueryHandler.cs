using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Features.Domains.ClassSessions.DTOs;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.ClassSessions.Queries;
public record GetSessionsByBranchAndDateQuery(int BranchId, DateOnly Date):IRequest<List<ClassSessionDTO>>;
public class GetSessionsByBranchAndDateQueryHandler
    :IRequestHandler<GetSessionsByBranchAndDateQuery, List<ClassSessionDTO>>
{
    private readonly TitanFitnessDbContext _context;
    public GetSessionsByBranchAndDateQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<List<ClassSessionDTO>> Handle(
        GetSessionsByBranchAndDateQuery request, CancellationToken cancellationToken)
    {
        var date=request.Date.ToDateTime(TimeOnly.MinValue);

        return await _context.ClassSessions
            .AsNoTracking()
            .Where(s=>s.BranchId==request.BranchId&&s.SessionDate==date)
            .OrderBy(s=>s.StartTime)
            .Select(s=>new ClassSessionDTO(
                s.SessionId,
                s.ClassName,
                s.BranchId,
                _context.Branches.Where(b=>b.BranchId==s.BranchId).Select(b=>b.BranchName).First(),
                s.StudioId,
                _context.Branches.SelectMany(b=>b.Studios)
                    .Where(st=>st.StudioId==s.StudioId).Select(st=>st.StudioName).First(),
                s.TrainerId,
                _context.Trainers.Where(t=>t.TrainerId==s.TrainerId).Select(t=>t.TrainerName).First(),
                s.SessionDate,
                s.StartTime,
                s.DurationInMinutes,
                s.CapacityLimit,
                s.Bookings.Count(b=>b.Status==BookingStatus.Confirmed),
                s.Bookings.Count(b=>b.Status==BookingStatus.Waitlisted),
                s.Status.ToString(),
                s.Description,
                new List<BookingDTO>() // 
            ))
            .ToListAsync(cancellationToken);
    }
}
