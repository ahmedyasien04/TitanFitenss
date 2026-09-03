using MediatR;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.ClassSessions.DTOs;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.ClassSessions.Queries;

public record GetSessionByIdQuery(int SessionId):IRequest<ClassSessionDTO>;

public class GetSessionByIdQueryHandler:IRequestHandler<GetSessionByIdQuery, ClassSessionDTO>
{
    private readonly TitanFitnessDbContext _context;

    public GetSessionByIdQueryHandler(TitanFitnessDbContext context)
    {
        _context=context;
    }
    public async Task<ClassSessionDTO> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var session=await _context.ClassSessions
            .AsNoTracking()
            .Where(s=>s.SessionId==request.SessionId)
            .Select(s=>new ClassSessionDTO(
                s.SessionId,
                s.ClassName,
                s.BranchId,
                _context.Branches.Where(b=>b.BranchId==s.BranchId).Select(b=>b.BranchName).First(),
                s.StudioId,
                _context.Branches
                    .SelectMany(b=>b.Studios)
                    .Where(st=>st.StudioId==s.StudioId)
                    .Select(st=>st.StudioName)
                    .First(),
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
                s.Bookings.Select(b=>new BookingDTO(
                    b.BookingId,
                    b.MemberId,
                    _context.Members.Where(m=>m.MemberId==b.MemberId).Select(m=>m.FullName).First(),
                    b.BookedOn,
                    b.Status.ToString(),
                    b.WaitlistPosition,
                    b.NotesForTrainer
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (session is null)
            throw new NotFoundException(nameof(ClassSession), request.SessionId);

        return session;
    }
}
