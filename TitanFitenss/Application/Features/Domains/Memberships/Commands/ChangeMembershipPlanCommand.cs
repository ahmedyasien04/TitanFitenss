using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.MembershipAggregate;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Memberships.Commands;
public record ChangeMembershipPlanCommand(
    int CurrentMembershipId,
    int NewPlanId,
    bool EffectiveImmediately
):IRequest<int>;
public class ChangeMembershipPlanCommandValidator:AbstractValidator<ChangeMembershipPlanCommand>
{
    public ChangeMembershipPlanCommandValidator()
    {
        RuleFor(x=>x.CurrentMembershipId).GreaterThan(0);
        RuleFor(x=>x.NewPlanId).GreaterThan(0);
    }
}
public class ChangeMembershipPlanCommandHandler:IRequestHandler<ChangeMembershipPlanCommand, int>
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly IPlanRepository _planRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeMembershipPlanCommandHandler(
        IMembershipRepository membershipRepository, IPlanRepository planRepository, IUnitOfWork unitOfWork)
    {
        _membershipRepository=membershipRepository;
        _planRepository=planRepository;
        _unitOfWork=unitOfWork;
    }

    public async Task<int> Handle(ChangeMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var current=await _membershipRepository.GetByIdAsync(request.CurrentMembershipId, cancellationToken)
            ?? throw new NotFoundException(nameof(Membership), request.CurrentMembershipId);

        if (current.Status==MembershipStatus.Cancelled)
            throw new BusinessRuleException("Cancelled memberships cannot be renewed or changed.");

        var newPlan=await _planRepository.GetByIdAsync(request.NewPlanId, cancellationToken)
            ?? throw new NotFoundException(nameof(Plan), request.NewPlanId);

        if (!newPlan.IsPublished)
            throw new BusinessRuleException("Cannot move a membership onto a plan that is not published.");

        DateTime newStartDate;

        if (request.EffectiveImmediately)
        {
            if (current.Status!=MembershipStatus.Cancelled&&current.Status!=MembershipStatus.Expired)
            {
                current.Cancel();
                _membershipRepository.Update(current);
            }

            newStartDate=DateTime.UtcNow.Date;
        }
        else
        {
            newStartDate=current.EndDate;
        }

        var newMembership=new Membership(current.MemberId, newPlan, DateTime.UtcNow, newStartDate);

        await _membershipRepository.AddAsync(newMembership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newMembership.MembershipId;
    }
}
