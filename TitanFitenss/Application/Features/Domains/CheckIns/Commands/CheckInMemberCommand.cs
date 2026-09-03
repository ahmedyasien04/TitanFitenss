using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Application.Features.Domains.CheckIns.DTOs;
using TitanFitenss.Domain.BranchAggregate;
using TitanFitenss.Domain.CheckInAggregate;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.CheckIns.Commands;
public record CheckInMemberCommand(int MemberId, int BranchId):IRequest<CheckInDTO>;

public class CheckInMemberCommandValidator:AbstractValidator<CheckInMemberCommand>
{
    public CheckInMemberCommandValidator()
    {
        RuleFor(x=>x.MemberId).GreaterThan(0);
        RuleFor(x=>x.BranchId).GreaterThan(0);
    }
}
public class CheckInMemberCommandHandler:IRequestHandler<CheckInMemberCommand, CheckInDTO>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly ICheckInRepository _checkInRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CheckInMemberCommandHandler(
        IMemberRepository memberRepository,
        IBranchRepository branchRepository,
        IMembershipRepository membershipRepository,
        ICheckInRepository checkInRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository=memberRepository;
        _branchRepository=branchRepository;
        _membershipRepository=membershipRepository;
        _checkInRepository=checkInRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task<CheckInDTO> Handle(CheckInMemberCommand request, CancellationToken cancellationToken)
    {
        var member=await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var branch=await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Branch), request.BranchId);

        var now=DateTime.UtcNow;
        var activeMembership=await _membershipRepository
            .GetActiveMembershipByMemberIdAsync(member.MemberId, cancellationToken);

        var (isGranted, refusalReason)=Evaluate(activeMembership, request.BranchId, member.HomeBranchId, now);

        var checkIn=isGranted
            ? CheckIn.CreateGranted(member.MemberId, request.BranchId, now)
            :CheckIn.CreateRefused(member.MemberId, request.BranchId, now, refusalReason!);

        await _checkInRepository.AddAsync(checkIn, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CheckInDTO(
            checkIn.CheckInId, checkIn.MemberId, checkIn.BranchId, branch.BranchName,
            checkIn.CheckInDateTime, checkIn.Result.ToString(), checkIn.RefusalReason);
    }
    private static (bool IsGranted, string? RefusalReason) Evaluate(
        Membership? membership, int branchId, int homeBranchId, DateTime now)
    {
        if (membership is null)
            return (false, "No active membership on file.");

        if (membership.Status==MembershipStatus.Pending)
            return (false, "This membership has not started yet.");

        if (membership.Status==MembershipStatus.Frozen)
            return (false, "This membership is currently frozen.");

        if (membership.Status==MembershipStatus.Expired)
            return (false, "This membership has expired.");

        if (membership.Status==MembershipStatus.Cancelled)
            return (false, "This membership was cancelled.");

        if (!membership.IsActiveOn(now))
            return (false, "This membership is not currently active.");

        if (!membership.CanAccessBranch(branchId, homeBranchId))
            return (false, "This membership does not grant access to this branch.");

        return (true, null);
    }
}
