using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.Interfaces;
using TitanFitenss.Infrastructure.Persistence;
namespace TitanFitenss.Application.Features.Domains.ClassSessions.Commands;
public record BookMemberCommand(int SessionId, int MemberId, string? NotesForTrainer):IRequest<int>;
public class BookMemberCommandValidator:AbstractValidator<BookMemberCommand>
{
    public BookMemberCommandValidator()
    {
        RuleFor(x=>x.SessionId).GreaterThan(0);
        RuleFor(x=>x.MemberId).GreaterThan(0);
        RuleFor(x=>x.NotesForTrainer).MaximumLength(500);
    }
}

public class BookMemberCommandHandler:IRequestHandler<BookMemberCommand, int>
{
    private readonly IClassSessionRepository _classSessionRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TitanFitnessDbContext _context;
    public BookMemberCommandHandler(
        IClassSessionRepository classSessionRepository,
        IMemberRepository memberRepository,
        IMembershipRepository membershipRepository,
        IUnitOfWork unitOfWork,
        TitanFitnessDbContext context)
    {
        _classSessionRepository=classSessionRepository;
        _memberRepository=memberRepository;
        _membershipRepository=membershipRepository;
        _unitOfWork=unitOfWork;
        _context=context;
    }
    public async Task<int> Handle(BookMemberCommand request, CancellationToken cancellationToken)
    {
        var session=await _classSessionRepository.GetByIdAsync(request.SessionId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClassSession), request.SessionId);

        var member=await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var activeMembership=await _membershipRepository
            .GetActiveMembershipByMemberIdAsync(member.MemberId, cancellationToken);

        if (activeMembership is null || !activeMembership.IsActiveOn(DateTime.UtcNow))
            throw new BusinessRuleException("This member does not have an active membership and cannot be booked.");

        var sessionStartsAt=session.SessionDate.Date+session.StartTime;
        if (sessionStartsAt<=DateTime.UtcNow)
            throw new BusinessRuleException("This session has already started or finished.");
        var sessionStart=session.StartTime;
        var sessionEnd=session.StartTime+TimeSpan.FromMinutes(session.DurationInMinutes);
        var otherBookedSessions=await _context.ClassSessions
            .AsNoTracking()
            .Where(cs=>cs.SessionId!=session.SessionId
                &&cs.SessionDate==session.SessionDate
                &&cs.Status!=SessionStatus.Cancelled
                &&cs.Bookings.Any(b=>b.MemberId==member.MemberId&&b.Status!=BookingStatus.Cancelled))
            .ToListAsync(cancellationToken);

        var hasOverlap=otherBookedSessions.Any(other=>
        {
            var otherStart=other.StartTime;
            var otherEnd=other.StartTime+TimeSpan.FromMinutes(other.DurationInMinutes);
            return sessionStart<otherEnd&&otherStart<sessionEnd;
        });

        if (hasOverlap)
            throw new BusinessRuleException("This member is already booked onto an overlapping session.");

        var booking=session.BookMember(member.MemberId, DateTime.UtcNow, request.NotesForTrainer);

        _classSessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.BookingId;
    }
}
