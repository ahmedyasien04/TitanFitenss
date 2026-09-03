using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.ClassSessionAggregate;
using TitanFitenss.Domain.TrainerAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.ClassSessions.Commands;
public record ScheduleClassSessionCommand(
    string ClassName,
    int BranchId,
    int StudioId,
    int TrainerId,
    DateOnly SessionDate,
    TimeSpan StartTime,
    int DurationInMinutes,
    int CapacityLimit,
    string? Description
):IRequest<int>;

public class ScheduleClassSessionCommandValidator:AbstractValidator<ScheduleClassSessionCommand>
{
    public ScheduleClassSessionCommandValidator()
    {
        RuleFor(x=>x.ClassName).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.BranchId).GreaterThan(0);
        RuleFor(x=>x.StudioId).GreaterThan(0);
        RuleFor(x=>x.TrainerId).GreaterThan(0);
        RuleFor(x=>x.DurationInMinutes).GreaterThan(0);
        RuleFor(x=>x.CapacityLimit).GreaterThan(0);
        RuleFor(x=>x.Description).MaximumLength(500);
    }
}
public class ScheduleClassSessionCommandHandler:IRequestHandler<ScheduleClassSessionCommand, int>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IClassSessionRepository _classSessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ScheduleClassSessionCommandHandler(
        IBranchRepository branchRepository,
        ITrainerRepository trainerRepository,
        IClassSessionRepository classSessionRepository,
        IUnitOfWork unitOfWork)
    {
        _branchRepository=branchRepository;
        _trainerRepository=trainerRepository;
        _classSessionRepository=classSessionRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task<int> Handle(ScheduleClassSessionCommand request, CancellationToken cancellationToken)
    {
        var branch=await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.BranchId);

        var studio=branch.Studios.FirstOrDefault(s=>s.StudioId==request.StudioId)
            ?? throw new NotFoundException("Studio", request.StudioId);

        if (request.CapacityLimit>studio.Capacity)
            throw new BusinessRuleException(
                $"Capacity ({request.CapacityLimit}) cannot exceed the studio's capacity ({studio.Capacity}).");

        var trainer=await _trainerRepository.GetByIdAsync(request.TrainerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Trainer), request.TrainerId);

        if (!trainer.IsActive)
            throw new BusinessRuleException("Cannot schedule a session with an inactive trainer.");

        var sessionDateTime=request.SessionDate.ToDateTime(TimeOnly.MinValue);
        var newRangeStart=request.StartTime;
        var newRangeEnd=request.StartTime+TimeSpan.FromMinutes(request.DurationInMinutes);

        var trainerSessionsThatDay=await _classSessionRepository
            .GetSessionsByTrainerAsync(request.TrainerId, sessionDateTime, cancellationToken);

        if (HasOverlap(trainerSessionsThatDay, newRangeStart, newRangeEnd))
            throw new BusinessRuleException("This trainer already has an overlapping session at that time.");

        var branchSessionsFromThatDay=await _classSessionRepository
            .GetUpcomingSessionsByBranchAsync(request.BranchId, sessionDateTime, cancellationToken);

        var studioSessionsThatDay=branchSessionsFromThatDay
            .Where(s=>s.StudioId==request.StudioId&&s.SessionDate.Date==sessionDateTime.Date);

        if (HasOverlap(studioSessionsThatDay, newRangeStart, newRangeEnd))
            throw new BusinessRuleException("This studio already has an overlapping session at that time.");

        var session=new ClassSession(
            request.ClassName, request.BranchId, request.StudioId, request.TrainerId,
            sessionDateTime, request.StartTime, request.DurationInMinutes,
            request.CapacityLimit, request.Description);

        await _classSessionRepository.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return session.SessionId;
    }

    private static bool HasOverlap(IEnumerable<ClassSession> sessions, TimeSpan newStart, TimeSpan newEnd)
    {
        foreach (var existing in sessions)
        {
            if (existing.Status==SessionStatus.Cancelled) continue;

            var existingStart=existing.StartTime;
            var existingEnd=existing.StartTime+TimeSpan.FromMinutes(existing.DurationInMinutes);

            if (newStart<existingEnd&&existingStart<newEnd)
                return true;
        }
        return false;
    }
}
