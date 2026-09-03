using MediatR;
using FluentValidation;
using TitanFitenss.Application.Common.Exceptions;
using TitanFitenss.Domain.PlanAggregate;
using TitanFitenss.Domain.ValueObjects;
using TitanFitenss.Domain.Interfaces;
namespace TitanFitenss.Application.Features.Domains.Plans.Commands;
public record UpdatePlanCommand(
    int PlanId,
    string PlanName,
    decimal Price,
    int DurationInMonths,
    int MaxFreezeDays,
    int MaxNumberOfFreezes,
    int GuestPassQuota,
    AccessScope AccessScope
):IRequest;
public class UpdatePlanCommandValidator:AbstractValidator<UpdatePlanCommand>
{
    public UpdatePlanCommandValidator()
    {
        RuleFor(x=>x.PlanId).GreaterThan(0);
        RuleFor(x=>x.PlanName).NotEmpty().MaximumLength(50);
        RuleFor(x=>x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.DurationInMonths).GreaterThan(0);
        RuleFor(x=>x.MaxFreezeDays).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.MaxNumberOfFreezes).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.GuestPassQuota).GreaterThanOrEqualTo(0);
        RuleFor(x=>x.AccessScope).IsInEnum();
    }
}
public class UpdatePlanCommandHandler:IRequestHandler<UpdatePlanCommand>
{
    private readonly IPlanRepository _planRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePlanCommandHandler(IPlanRepository planRepository, IUnitOfWork unitOfWork)
    {
        _planRepository=planRepository;
        _unitOfWork=unitOfWork;
    }
    public async Task Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var plan=await _planRepository.GetByIdAsync(request.PlanId, cancellationToken)
            ?? throw new NotFoundException(nameof(Plan), request.PlanId);

        plan.UpdatePlanDetails(
            request.PlanName,
            request.Price,
            request.DurationInMonths,
            request.MaxFreezeDays,
            request.MaxNumberOfFreezes,
            request.GuestPassQuota,
            request.AccessScope);

        _planRepository.Update(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
