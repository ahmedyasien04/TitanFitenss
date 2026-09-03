using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.MemberAggregate;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Memberships.Commands;
public record PurchaseMembershipCommand(int MemberId, int PlanId, DateOnly StartDate):IRequest<int>;

public class PurchaseMembershipCommandValidator:AbstractValidator<PurchaseMembershipCommand>
{
    public PurchaseMembershipCommandValidator()
    {
        RuleFor(x=>x.MemberId).GreaterThan(0);
        RuleFor(x=>x.PlanId).GreaterThan(0);
    }
}
public class PurchaseMembershipCommandHandler:IRequestHandler<PurchaseMembershipCommand, int>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IPlanRepository _planRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseMembershipCommandHandler(
        IMemberRepository memberRepository,
        IPlanRepository planRepository,
        IMembershipRepository membershipRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository=memberRepository;
        _planRepository=planRepository;
        _membershipRepository=membershipRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task<int> Handle(PurchaseMembershipCommand request, CancellationToken cancellationToken)
    {
        var member=await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var plan=await _planRepository.GetByIdAsync(request.PlanId, cancellationToken)
            ?? throw new NotFoundException(nameof(Plan), request.PlanId);

        if (!plan.IsPublished)
            throw new BusinessRuleException("Cannot purchase a plan that is not published.");

        var startDateTime = request.StartDate.ToDateTime(TimeOnly.MinValue);
        var membership = new Membership(member.MemberId, plan, DateTime.UtcNow, startDateTime);

        var existingMemberships=await _membershipRepository
            .GetMembershipsByMemberIdAsync(member.MemberId, cancellationToken);

        var overlaps=existingMemberships.Any(existing=>
            existing.Status!=MembershipStatus.Cancelled&&
            membership.StartDate<=existing.EndDate&&
            existing.StartDate<=membership.EndDate);

        if (overlaps)
            throw new BusinessRuleException(
                "This member already holds a membership that covers part of this period.");

        await _membershipRepository.AddAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return membership.MembershipId;
    }
}
